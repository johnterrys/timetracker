using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeCats.Models
{
    public class EvalTemplate
    {
        [Key]
        public int evalTemplateID { get; set; }
        [Required]
        public int userID { get; set; }
        public string templateName { get; set; }
        public bool inUse { get; set; }

        public List<EvalTemplateQuestionCategory> categories { get; set; }
        public List<EvalTemplateQuestion> templateQuestions { get; set; }
    }
}