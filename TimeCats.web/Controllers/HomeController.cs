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

        /// <summary>
        ///     Returns the currently logged in user's userID.
        /// </summary>
        /// <returns></returns>
        public int GetUserID()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null)
            {
                return user.userID;
            }

            return 0;
        }

        /// <summary>
        ///     Returns the currently logged in user's type. Otherwise returns null character.
        /// </summary>
        /// <returns></returns>
        public char GetUserType()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null)
            {
                return user.type;
            }

            return '\0'; //Null character literal
        }

        /// <summary>
        ///     Returns true if the currently logged in user is an Admin
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin()
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null)
            {
                return user.type == 'A';
            }

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
            //get all coures by user and then check if one of those courseID's is the courseID passed in.
            var courses = _courseService.GetCoursesByUser(user);
            if (courses.Any(c => c.courseID == courseID))
            {
                return true;
            }

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
        ///     Returns true if the user is already in a group for the project associated with the given group
        /// </summary>
        /// <returns></returns>
        public bool IsActiveStudentInGroup(int groupID)
        {
            var user = HttpContext.Session.GetObjectFromJson<User>("user");

            if (user != null) return _userService.IsActiveUserInGroup(user.userID, groupID);
            return false;
        }

        #region Helper Functions

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
