using System.Collections.Generic;

namespace TimeCats.Models
{
    public class Project
    {
        public int projectID { get; set; }

        public string projectName { get; set; }

        public bool isActive { get; set; }

        public string description { get; set; }

        public List<Group> groups { get; set; }

        public int CourseID { get; set; }
    }
}