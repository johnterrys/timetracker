using System.Collections.Generic;

namespace TimeCats.Models
{
    public class EmptyEval
    {
        public int evalID { get; set; }
        public int evalTemplateQuestionID { get; set; }
        public int evalTemplateQuestionCategoryID { get; set; }
        public int groupID { get; set; }
        public bool isComplete { get; set; }

        public int evalTemplateID { get; set; }
        public int number { get; set; }
        public List<EvalTemplateQuestionCategory> categories { get; set; }
        public List<EvalTemplateQuestion> templateQuestions { get; set; }
        public List<User> users { get; set; }
        public List<EvalResponse> responses { get; set; }
    }
}