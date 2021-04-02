using System.ComponentModel.DataAnnotations;

namespace TimeCats.Models
{
    public class EvalTemplateQuestion
    {
        [Key]
        public int evalTemplateQuestionID { get; set; }
        [Required]
        public int evalTemplateID { get; set; }
        [Required]
        public int evalTemplateQuestionCategoryID { get; set; }
        public int number { get; set; }
        public string questionType { get; set; }
        public string questionText { get; set; }
        public int evalID { get; set; }
    }
}