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
    public class HomeController : Controller
    {
        protected readonly StudentTimeTrackerService _timeTrackerService;
        protected readonly CourseService _courseService;
        protected readonly EvalService _evalService;
        protected readonly GroupService _groupService;
        protected readonly ProjectService _projectService;
        protected readonly TimeService _timeService;
        protected readonly UserService _userService;

        public HomeController(IServiceProvider serviceProvider)
        {
            _timeTrackerService = serviceProvider.GetRequiredService<StudentTimeTrackerService>();
            _courseService = serviceProvider.GetRequiredService<CourseService>();
            _evalService = serviceProvider.GetRequiredService<EvalService>();
            _groupService = serviceProvider.GetRequiredService<GroupService>();
            _projectService = serviceProvider.GetRequiredService<ProjectService>();
            _timeService = serviceProvider.GetRequiredService<TimeService>();
            _userService = serviceProvider.GetRequiredService<UserService>();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Helper Functions

        /// <summary>
        ///     Returns a hashed version of the passed password
        /// </summary>
        /// <returns></returns>
        public static string GenerateHash(string password)
        {
            var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        ///     Returns the courseID for the passed groupID
        /// </summary>
        /// <returns></returns>
        public int GetCourseForGroup(int groupID)
        {
            return _courseService.GetCourseForGroup(groupID);
        }

        /// <summary>
        ///     Returns the courseID for the passed projectID
        /// </summary>
        /// <returns></returns>
        public int GetCourseForProject(int projectID)
        {
            var courseId = _projectService.GetProjectById(projectID).CourseID;
            return courseId;
        }

        /// <summary>
        ///     Returns the currently logged in user's userID.
        /// </summary>
        /// <returns></returns>
        public int GetUserID()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return user.userID;

            return 0;
        }

        /// <summary>
        ///     Returns the currently logged in user's type. Otherwise returns null character.
        /// </summary>
        /// <returns></returns>
        public char GetUserType()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return user.type;

            return '\0'; //Null character literal
        }

        /// <summary>
        ///     Returns true if the currently logged in user is an Admin
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return user.type == 'A';

            return false;
        }

        /// <summary>
        ///     Returns true if the logged in user is the instructor for the passed courseID
        /// </summary>
        /// <returns></returns>
        public bool IsInstructorForCourse(int courseID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");
            // make sure the user is an instructor
            if (user.type != 'I')
            {
                return false;
            }

            var courses = _courseService.GetCoursesByUser(user);
            if (courses.Any(c => c.courseID == courseID))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Returns true if the logged in user is the instructor for the passed evalTemplateID
        /// </summary>
        /// <returns></returns>
        public bool IsInstructorForEval(int evalTemplateID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return user.userID == _evalService.GetInstructorForEval(evalTemplateID);

            return false;
        }

        /// <summary>
        ///     Returns true if the logged in user is a student for the passed courseID
        /// </summary>
        /// <returns></returns>
        public bool IsStudentInCourse(int courseID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");
            var courses = _courseService.GetCoursesByUser(user);

            return courses.Any(c => c.courseID == courseID);
        }

        /// <summary>
        ///     Returns true if the logged in user is a student for the passed courseID
        /// </summary>
        /// save
        /// <returns></returns>
        public bool UserIsStudentInCourse(int userID, int courseID)
        {
            if (userID != 0 && courseID != 0) return _userService.UserIsInCourse(courseID, userID);

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
        public bool IsActiveStudentInGroup(int groupID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return _userService.IsActiveUserInGroup(user.userID, groupID);
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
        ///     Returns true if the logged in user has timecard entries for the passed group
        /// </summary>
        /// <returns></returns>
        public bool UserHasTimeInGroup(int groupID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return _timeService.UserHasTimeInGroup(user.userID, groupID);

            return false;
        }

        /// <summary>
        ///     Returns the logged in user if there is one.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CheckSession()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");
            if (user != null)
                return Ok(user);
            return Unauthorized(); //Unauthorized (401) if there isn't a user in the session
        }

        #endregion
    }
}
