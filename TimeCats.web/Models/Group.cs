using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    public class Group
    {
        [Key]
        public int groupID { get; set; }

        [Required]
        public string groupName { get; set; }

        public bool isActive { get; set; }

        [NotMapped]
        public List<User> users { get; set; }
        public List<UserGroup> UserGroups { get; set; }

        [Required]
        public int projectID { get; set; }
        public Project Project { get; set; }

        [NotMapped]
        public int evalID { get; set; }
    }
}
