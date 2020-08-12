using System.ComponentModel.DataAnnotations;

namespace TimeCats.Models
{
    public class EvalTemplateQuestionCategory
    {
        [Key]
        public int evalTemplateQuestionCategoryID { get; set; }
        [Required]
        public int evalTemplateID { get; set; }
        public string categoryName { get; set; }
        public int number { get; set; }
    }
}