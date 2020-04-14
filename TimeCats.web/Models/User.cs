using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    public class User
    {
        [Key]
        public int userID { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; } //TODO: This should be a hash we also need a salt

        [Required]
        public byte[] Salt { get; set; }

        [NotMapped]
        public string newPassword { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public char type { get; set; } //TODO: Update this to use an ENUMERABLE

        public bool isActive { get; set; }

        public List<TimeCard> timecards { get; set; }

        public List<UserGroup> UserGroups { get; set; }

        [NotMapped]
        public List<Course> Courses { get; set; }
        public List<UserCourse> UserCourses { get; set; }
    }
}
