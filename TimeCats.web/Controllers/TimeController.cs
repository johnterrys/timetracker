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
    public class TimeController : HomeController
    {
        public TimeController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {}

        /// <summary>
        ///     Creates a TimeCard and returns the timeSlotID
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public IActionResult CreateTimeCard([FromBody] object json)
        {
            var JsonString = json.ToString();

            var timeCard = JsonConvert.DeserializeObject<TimeCard>(JsonString);
            var course = _courseService.GetCourseForGroup(timeCard.groupID);

            if (IsAdmin() ||
                IsInstructorForCourse(course.courseID) ||
                IsStudentInCourse(course.courseID))
            {
                if (GetUserType() == 'S' && GetUserID() == timeCard.userID)
                {
                    timeCard.timeslotID = (int) _timeService.CreateTimeCard(timeCard);
                }
                else
                {
                    timeCard.timeslotID = (int) _timeService.CreateTimeCard(timeCard);
                }

                if (timeCard.timeslotID > 0)
                {
                    return Ok(timeCard.timeslotID);
                }

                return StatusCode(500);
            }

            return Unauthorized();
        }

        public IActionResult DeleteTimeCard([FromBody] object json)
        {
            var JsonString = json.ToString();

            var timeCard = JsonConvert.DeserializeObject<TimeCard>(JsonString);
            var course = _courseService.GetCourseForGroup(timeCard.groupID);

            if (IsAdmin() ||
                IsInstructorForCourse(course.courseID) ||
                IsStudentInCourse(course.courseID))
            {
                if (GetUserType() == 'S' && GetUserID() == timeCard.userID)
                {
                    /*Changed to DELETE*/
                    timeCard.timeslotID = (int) _timeService.DeleteTimeCard(timeCard);
                    return StatusCode(200);
                }

                /*Changed to DELETE*/
                timeCard.timeslotID = (int) _timeService.DeleteTimeCard(timeCard);
                return StatusCode(200);
            }

            return Unauthorized();
        }

        [HttpPost]
        public IActionResult SaveTime([FromBody] object json)
        {
            var JsonString = json.ToString();

            var timecard = JsonConvert.DeserializeObject<TimeCard>(JsonString);
            var course = _courseService.GetCourseForGroup(timecard.groupID);

            //  Check the time input on the server side to stop any manual
            // posts that are not valid.
            DateTime timeIn;
            DateTime timeOut;

            //  Is time in a date?,  is time out a date?
            //  are hours negative?, is time out a future date?
            if (!DateTime.TryParse(timecard.timeIn, out timeIn) ||
                !DateTime.TryParse(timecard.timeOut, out timeOut) ||
                timeOut.CompareTo(timeIn) < 0 ||
                timeOut > DateTime.Now ||
                timeIn > DateTime.Now)
            {
                return BadRequest("Invalid time entered");
            }

            if (IsAdmin() || GetUserID() == timecard.userID ||
                IsInstructorForCourse(course.courseID))
            {
                if (_timeService.SaveTime(timecard))
                {
                    return Ok();
                }

                return StatusCode(500);
            }

            return Unauthorized();
        }
    }
}
