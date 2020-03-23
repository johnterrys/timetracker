using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TimeCats.Models;

namespace TimeCats.DTOs
{
    /// <summary>
    ///   Data transfer object for Project
    /// </summary>
    public class ProjectDTO
    {
        [JsonIgnore]
        protected Project _project;

        public int projectID { get; set; }
        public string projectName { get; set; }
        public bool isActive { get; set; }
        public string description { get; set; }
        public List<GroupDTO> groups { get; set; }
        public int CourseID { get; set; }
        public CourseDTO Course { get; set; }

        /// <summary>
        ///   Create a ProjectDTO for the given project. By default, the course
        ///   and groups are not included
        /// </summary>
        public ProjectDTO(Project that) {
            _project = that;

            this.projectID = that.projectID;
            this.projectName = that.projectName;
            this.isActive = that.isActive;
            this.description = that.description;
            this.CourseID = that.CourseID;

            // not included by default
            this.Course = null;
            this.groups = null;
        }

        /// <summary>
        ///   Include the project's groups in this DTO. May cause cyclic
        ///   reference issues
        /// </summary>
        public ProjectDTO WithGroups()
        {
            this.groups = new List<GroupDTO>();
            foreach (Group group in _project.groups)
            {
                this.groups.Add(new GroupDTO(group));
            }

            return this;
        }

        /// <summary>
        ///   Include the project's course in this DTO. May cause cyclic
        ///   reference issues
        /// </summary>
        public ProjectDTO WithCourse()
        {
            this.Course = new CourseDTO(_project.Course);
            return this;
        }
    }
}
