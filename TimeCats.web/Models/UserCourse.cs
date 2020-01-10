using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    public class UserCourse
    {
        public int userID { get; set; }
        public User User { get; set; }
        
        public int courseID { get; set; }
        public Course Course { get; set; }
        
        [NotMapped]
        public bool isActive { get; set; }
    }
}