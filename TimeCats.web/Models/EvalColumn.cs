using System.ComponentModel.DataAnnotations;

namespace TimeCats.Models
{
    public class EvalColumn
    {
        [Key]
        public int originalID { get; set; }
        [Required]
        public int evalID { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
    }
}