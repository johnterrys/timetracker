using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeCats.Models
{
    public class EmptyEval
    {
        [Key]
        public int evalID { get; set; }
        [Required]
        public int evalTemplateQuestionID { get; set; }
        [Required]
        public int evalTemplateQuestionCategoryID { get; set; }
        [Required]
        public int groupID { get; set; }
        public bool isComplete { get; set; }

        [Required]
        public int evalTemplateID { get; set; }
        public int number { get; set; }
        public List<EvalTemplateQuestionCategory> categories { get; set; }
        public List<EvalTemplateQuestion> templateQuestions { get; set; }
        public List<User> users { get; set; }
        public List<EvalResponse> responses { get; set; }
    }
}