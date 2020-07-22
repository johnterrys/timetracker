using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    [Table("UserCourses")]
    public class UserCourse
    {
        [Required]
        public int userID { get; set; }
        public User User { get; set; }
        
        [Required]
        public int courseID { get; set; }
        public Course Course { get; set; }
        public bool isActive { get; set; }
    }
}