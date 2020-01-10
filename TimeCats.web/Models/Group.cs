using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    public class Group
    {
        public int groupID { get; set; }

        public string groupName { get; set; }

        public bool isActive { get; set; }

        public List<User> users { get; set; }

        public int projectID { get; set; }
        public Project Project { get; set; }

        [NotMapped]
        public int evalID { get; set; }
    }
}