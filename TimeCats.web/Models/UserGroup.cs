using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    [Table("UserGroups")]
    public class UserGroup
    {
        public int userID { get; set; }
        public User User { get; set; }
        
        public int groupID { get; set; }
        public Group Group { get; set; }
    }
}