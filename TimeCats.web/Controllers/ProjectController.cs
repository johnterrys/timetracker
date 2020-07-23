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
    public class ProjectController : HomeController
    {
        public ProjectController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {}

        /// <summary>
        ///     Add a project for the passed course. Returns the projectID
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddProject([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            if (IsInstructorForCourse(course.courseID) || IsAdmin())
            {
                var project = new Project()
                {
                    projectID = _projectService.GetNewProjectID(),
                    projectName = "Default Project",
                    description = "This is the default project template.",
                                                        isActive = true,
                    CourseID = course.courseID
                };

                var p = _projectService.AddProject(project);
                if (project.projectID > 0) return Ok(project.projectID);
                return StatusCode(500); //Query Error
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Return a Project based on the ID. Returns a Project if successful 204 otherwise
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetProject([FromBody] object json)
        {
            var JsonString = json.ToString();
            var project = JsonConvert.DeserializeObject<Project>(JsonString);
            var course = _courseService.GetCourseForProject(project.projectID);

            if (IsAdmin() ||
                IsInstructorForCourse(course.courseID) ||
                IsStudentInCourse(course.courseID))
            {
                project = _projectService.GetProjectById(project.projectID);

                return Ok(new ProjectDTO(project).WithGroups());
            }

            return Unauthorized();
        }

        /// <summary>
        ///     Update a Project name or isActive status.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveProject([FromBody] object json)
        {
            var JsonString = json.ToString();
            var projectData = JsonConvert.DeserializeObject<Project>(JsonString);
            var course = _courseService.GetCourseForProject(projectData.projectID);

            if (IsAdmin() || IsInstructorForCourse(course.courseID))
            {
                var project = _projectService.GetProjectById(projectData.projectID);
                project.description = projectData.description;
                project.projectName = projectData.projectName;
                project.isActive = projectData.isActive;

                if (_projectService.SaveProject(project))
                {
                    return Ok();
                }

                return StatusCode(500); // Query failed
            }

            return Unauthorized(); // Not an Admin or the Instructor for the course, Unauthorized (401)
        }

        [HttpPost]
        public IEnumerable<Project> GetProjects([FromBody] object json)
        {
            var JsonString = json.ToString();
            var course = JsonConvert.DeserializeObject<Course>(JsonString);

            var projects = _projectService.GetProjectsByCourseId(course.courseID);

            return projects;
        }
    }
}
