using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    public class Project
    {
        public int projectID { get; set; }

        public string projectName { get; set; }

        public bool isActive { get; set; }

        public string description { get; set; }

        [NotMapped]
        public List<Group> groups { get; set; }
        public List<UserGroup> UserGroups { get; set; }

        public int CourseID { get; set; }
        public Course Course { get; set; }
    }
}