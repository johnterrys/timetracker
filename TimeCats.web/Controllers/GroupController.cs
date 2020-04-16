using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TimeCats.Session;
using TimeCats.Models;
using TimeCats.Services;
using TimeCats.DTOs;

namespace TimeCats.Controllers
{
    public class GroupController : HomeController
    {
        public GroupController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {}

        public IActionResult CreateGroup([FromBody] object json)
        {
            var JsonString = json.ToString();

            int groupID;
            var project = JsonConvert.DeserializeObject<Project>(JsonString);
            var course = _courseService.GetCourseForProject(project.projectID);

            // admins and instructors can create groups. Students can create
            // groups if they are in the course and not already in a group for
            // the project
            if (IsAdmin() || IsInstructorForCourse(course.courseID) ||
                (IsStudentInCourse(course.courseID) &&
                 !IsStudentInGroupForProject(project.projectID)))
            {
                // create the group
                groupID = _groupService.CreateGroup(new Group()
                {
                    groupName = "New Group",
                    projectID = project.projectID,
                    isActive = true
                });

                // if a student is creating the group, add them to it
                if (GetUserType() == 'S')
                {
                    if (!IsStudentInGroupForProject(project.projectID) &&
                        groupID > 0)
                    {
                        _groupService.JoinGroup(GetUserID(), groupID);
                    }
                    else
                    {
                        return StatusCode(403); //Student already part of group, unable to create a new one.
                    }
                }

                if (groupID > 0)
                {
                    return Ok(groupID);
                }

                return StatusCode(500); //Failed Query
            }

            return Unauthorized();
        }

        /// <summary>
        ///   Get a group
        /// </summary>
        /// <param name="json">A JSON string containing the groupID</param>
        [HttpPost]
        public IActionResult GetGroup([FromBody] object json)
        {
            var JsonString = json.ToString();
            var groupID = JsonConvert.DeserializeObject<Group>(JsonString).groupID;
            var group = _groupService.GetGroup(groupID);

            // Make sure that the user is part of the group's course
            var courseID = group.Project.CourseID;
            if (IsStudentInCourse(courseID) ||
                IsAdmin() ||
                IsInstructorForCourse(courseID))
            {
                var dto = new GroupDTO(group).WithUsers();
                foreach (var user in dto.users)
                {
                    user.WithTimeCards();
                }

                return Ok(dto);
            }

            return Unauthorized(); //Not allowed to view the group.
        }

        /// <summary>
        ///     Returns all users in the passed group.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetUsersForGroup([FromBody] object json)
        {
            var JsonString = json.ToString();
            var group = JsonConvert.DeserializeObject<Group>(JsonString);
            var course = _courseService.GetCourseForGroup(group.groupID);

            if (IsStudentInGroup(group.groupID) ||
                IsAdmin() ||
                IsInstructorForCourse(course.courseID))
            {
                var users = _userService.GetUsersForGroup(group.groupID);
                return Ok(users);
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult JoinGroup([FromBody] object json)
        {
            var JsonString = json.ToString();
            var uGroups = JsonConvert.DeserializeObject<UserGroup>(JsonString);

            var user = HttpContext.Session.GetObjectFromJson<User>("user");
            var course = _courseService.GetCourseForGroup(uGroups.groupID);

            Console.WriteLine("am in ur thing");

            if (IsStudentInOtherGroup(uGroups.groupID))
            {
                Console.WriteLine("student is in other group");
                return StatusCode(403);
            }

            if (IsStudentInCourse(course.courseID))
            {
                Console.WriteLine("student is in ur course");

                if (IsStudentInGroup(uGroups.groupID))
                {
                    Console.WriteLine("student is in ur group");

                    if (_groupService.ReJoinGroup(user.userID, uGroups.groupID))
                    {
                        Console.WriteLine("rejoining ur group");

                        return NoContent();
                    }

                    return StatusCode(500); //Query failed
                }

                var groupID = _groupService.JoinGroup(user.userID, uGroups.groupID);
                if (groupID > 0)
                {
                    Console.WriteLine("joining ur group");

                    return Ok(groupID);
                }

                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //User not in Course
        }

        [HttpPost]
        public IActionResult LeaveGroup([FromBody] object json)
        {
            var JsonString = json.ToString();
            var group = JsonConvert.DeserializeObject<Group>(JsonString);

            if (IsActiveStudentInGroup(group.groupID))
            {
                if (UserHasTimeInGroup(group.groupID))
                {
                    //Mark the user as inactive in the group if they have existing time entries
                    if (_groupService.LeaveGroup(GetUserID(), group.groupID)) return Ok();
                    return StatusCode(500); //Query failed
                }

                //Actually remove the user from the group if they don't have any time entries yet.
                if (_groupService.DeleteFromGroup(GetUserID(), group.groupID)) return NoContent();
                return StatusCode(500); //Query failed
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Updates a Group name.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveGroup([FromBody] object json)
        {
            var JsonString = json.ToString();

            var group = JsonConvert.DeserializeObject<Group>(JsonString);
            var course = _courseService.GetCourseForGroup(group.groupID);

            if (IsAdmin() || IsInstructorForCourse(course.courseID) ||
                IsActiveStudentInGroup(group.groupID))
            {
                if (_groupService.SaveGroup(group)) return Ok();
                return StatusCode(500); // Query failed
            }

            return
                Unauthorized(); // Not an Admin or the Instructor for the course, or a student in the group, Unauthorized (401)
        }

        /// <summary>
        ///     Returns true if the user is already in a group for the given project
        /// </summary>
        /// <returns></returns>
        public bool IsStudentInGroupForProject(int projectID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return _groupService.IsUserInGroupForProject(user.userID, projectID);
            return false;
        }

        /// <summary>
        ///     Returns true if the user is already in a group for the project associated with the given group, ACTIVE OR INACTIVE
        /// </summary>
        /// <returns></returns>
        public bool IsStudentInGroup(int groupID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return _userService.IsUserInGroup(user.userID, groupID);
            return false;
        }

        /// <summary>
        ///     Returns true if the user is already in a group for the project associated with the given group
        /// </summary>
        /// <returns></returns>
        public bool IsStudentInOtherGroup(int groupID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null)
            {
                return _groupService.IsUserInOtherGroup(user.userID, groupID);
            }

            return false;
        }

        /// <summary>
        ///     Returns true if the logged in user has timecard entries for the passed group
        /// </summary>
        /// <returns></returns>
        public bool UserHasTimeInGroup(int groupID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return _timeService.UserHasTimeInGroup(user.userID, groupID);

            return false;
        }
    }
}
