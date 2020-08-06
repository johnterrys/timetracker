using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeCats.Models
{
    public class AssignEvals
    {
        [Required]
        public List<int> projectIDs { get; set; }
        [Required]
        public int evalTemplateID { get; set; }
    }
}