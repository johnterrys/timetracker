using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    public class Project
    {
        [Key]
        public int projectID { get; set; }

        [Required]
        public string projectName { get; set; }

        public bool isActive { get; set; }

        [Required]
        public string description { get; set; }

        [NotMapped]
        public List<Group> groups { get; set; }

        [Required]
        public int CourseID { get; set; }
        public Course Course { get; set; }
    }
}
