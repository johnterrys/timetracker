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
    public class CourseController : HomeController
    {
        public CourseController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {}

        /// <summary>
        ///     Add a course. Returns the course ID
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddCourse([FromBody] object json)
        {
            var JsonString = json.ToString();

            if (GetUserType() == 'I' || IsAdmin())
            {
                var course = _courseService.AddCourse(new Course()
                {
                    courseID = _courseService.GetNextCourseID(),
                    courseName = "New Course",
                    InstructorId = 2,
                    isActive = true,
                    description = ""
                });

                if (course.courseID > 0)
                    return Ok(course.courseID);
                else
                    return StatusCode(500);
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Deletes the passed user and course association
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeleteUserCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var uCourse = JsonConvert.DeserializeObject<UserCourse>(JsonString);
            if (IsAdmin() || IsInstructorForCourse(uCourse.courseID))
            {
                if (_courseService.DeleteUserCourse(uCourse.userID, uCourse.courseID)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized(); //Not an Admin or the Instructor for the course, Unauthorized (401)
        }

        /// <summary>
        ///     Get a course and its projects and users
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            var retrievedCourse = _courseService.GetCourse(course.courseID);

            return Ok(retrievedCourse);
        }

        /// <summary>
        ///     Get a list of all the courses
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCourses()
        {
                var allCourses = _courseService.GetCourses();
                return Ok(allCourses);
        }

        /// <summary>
        ///     Get a list of this user's courses if they are an instructor
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetInstructorCourses()
        {
            var allCourses = _courseService.GetCourses();
            var userCourses = new List<Course>();

            foreach (var c in allCourses)
                if (IsInstructorForCourse(c.courseID))
                    userCourses.Add(c);
            return Ok(userCourses);
        }

        [HttpPost]
        public IActionResult GetInactiveStudentsInCourse([FromBody] object json)
        {
            var course = JsonConvert.DeserializeObject<Course>(json.ToString());
            var inactiveUsers = new List<User>();
            inactiveUsers = _courseService.GetInactiveUsersForCourse(course.courseID);
            return Ok(inactiveUsers);
        }

        [HttpPost]
        public IActionResult GetUsersForCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            var users = _userService.GetUsersForCourse(course.courseID);
            return Ok(users);
        }

        [HttpPost]
        public IActionResult JoinCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            var recordsUpdated = _courseService.RequestJoinCourse(GetUserID(), course.courseID);

            if (recordsUpdated > 0)
            {
                return Ok();
            }

            return StatusCode(500); //Query failed
        }

        [HttpPost]
        public IActionResult LeaveCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            if (IsStudentInCourse(course.courseID))
            {
                if (_courseService.LeaveCourse(course.courseID, GetUserID())) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult SaveUserInCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var uCourse = JsonConvert.DeserializeObject<UserCourse>(JsonString);

            if ((IsAdmin() || IsInstructorForCourse(uCourse.courseID)) &&
                UserIsStudentInCourse(uCourse.userID, uCourse.courseID))
            {
                if (_courseService.SaveUserInCourse(uCourse)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult DeleteUserFromCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var uCourse = JsonConvert.DeserializeObject<UserCourse>(JsonString);

            if ((IsAdmin() || IsInstructorForCourse(uCourse.courseID)) &&
                UserIsStudentInCourse(uCourse.userID, uCourse.courseID))
            {
                if (_courseService.DeleteFromCourse(uCourse.courseID, uCourse.userID)) return Ok();
                return StatusCode(500); //Query failed
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Updates a Course name
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveCourse([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            //Set course without instructor to admin
            if (course.InstructorId == 0)
            {
                //UserID of 1 is the default admin account
                course.InstructorId = 1;
            }

            if (IsAdmin() || IsInstructorForCourse(course.courseID))
            {
                if (_courseService.SaveCourse(course))
                {
                    return Ok();
                }
                else
                {
                    return StatusCode(500); //Query failed
                }
            }

            return Unauthorized(); //Not an Admin or the Instructor for the course, Unauthorized (401)
        }

        /// <summary>
        ///     Returns true if the logged in user is a student for the passed courseID
        /// </summary>
        /// save
        /// <returns></returns>
        public bool UserIsStudentInCourse(int userID, int courseID)
        {
            if (userID != 0 && courseID != 0)
            {
                return _userService.UserIsInCourse(courseID, userID);
            }

            return false;
        }
    }
}
