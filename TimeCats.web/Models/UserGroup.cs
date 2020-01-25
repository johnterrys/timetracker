using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    [Table("UserGroups")]
    public class UserGroup
    {
        [Required]
        public int userID { get; set; }
        public User User { get; set; }
        
        [Required]
        public int groupID { get; set; }
        public Group Group { get; set; }
    }
}