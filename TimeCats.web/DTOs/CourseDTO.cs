using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TimeCats.Models;

namespace TimeCats.DTOs
{
    /// <summary>
    ///   Data transfer object for Course
    /// </summary>
    public class CourseDTO
    {
        [JsonIgnore]
        protected Course _course;

        public int courseID { get; set; }
        public string courseName { get; set; }
        public string description { get; set; }
        public bool isActive { get; set; }
        public int InstructorId { get; set; }
        public string instructorName { get; set; }
        public List<ProjectDTO> projects { get; set; }

        // still need to add UserDTO
        //public UserDTO Instructor { get; set; }
        //public List<UserDTO> users { get; set; }

        public CourseDTO(Course that)
        {
            this._course = that;

            this.courseID = that.courseID;
            this.courseName = that.courseName;
            this.description = that.description;
            this.isActive = that.isActive;
            this.InstructorId = that.InstructorId;
            this.instructorName = that.instructorName;

            // not included by default
            this.projects = null;
            //this.users = null;
        }

        /// <summary>
        /// </summary>
        public CourseDTO WithProjects()
        {
            this.projects = new List<ProjectDTO>();
            foreach (Project project in _course.projects)
            {
                this.projects.Add(new ProjectDTO(project));
            }

            return this;
        }

        /*
        /// <summary>
        /// </summary>
        public CourseDTO WithUsers()
        {
            this.users = new List<UserDTO>();
            foreach (User user in _course.users)
            {
                this.users.Add(new ProjectDTO(user));
            }

            return this;
        }
        */
    }
}
