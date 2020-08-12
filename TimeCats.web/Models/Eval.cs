using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    public class Eval
    {
        [Key]
        public int evalID { get; set; }
        [Required]
        public int evalTemplateID { get; set; }
        [Required]
        public int groupID { get; set; }
        [Required]
        public int userID { get; set; }

        public int number { get; set; }
        public bool isComplete { get; set; }

        public List<EvalTemplateQuestionCategory> categories { get; set; }
        public List<EvalTemplateQuestion> templateQuestions { get; set; }
        public List<EvalResponse> responses { get; set; }
        public List<User> users { get; set; }
        public List<EvalColumn> evals { get; set; }

        //  States for Mr. Peterson
        [NotMapped]
        public Dictionary<int, int> columnSums { get; set; }
        [NotMapped]
        public Dictionary<int, double> userAvgerage { get; set; }
    }
}