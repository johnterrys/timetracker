using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    public class Course
    {
        public int courseID { get; set; }

        public string courseName { get; set; }

        public int instructorID { get; set; }

        public bool isActive { get; set; }

        public string description { get; set; }

        public string instructorName { get; set; }

        [NotMapped]
        public List<User> users { get; set; }
        public List<UserCourse> UserCourses { get; set; }

        public List<Project> projects { get; set; }
    }
}