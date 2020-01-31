using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    public class Course
    {
        [Key]
        public int courseID { get; set; }

        [Required]
        public string courseName { get; set; }
        
        [Required]
        public int InstructorId { get; set; }
        public User Instructor { get; set; }

        public bool isActive { get; set; }

        [Required]
        public string description { get; set; }

        [NotMapped]
        public string instructorName { get; set; }

        [NotMapped]
        public List<User> users { get; set; }
        public List<UserCourse> UserCourses { get; set; }

        public List<Project> projects { get; set; }
    }
}