using System.Collections.Generic;

namespace TimeCats.Models
{
    public class EvalTemplate
    {
        public int evalTemplateID { get; set; }
        public int userID { get; set; }
        public string templateName { get; set; }
        public bool inUse { get; set; }

        public List<EvalTemplateQuestionCategory> categories { get; set; }
        public List<EvalTemplateQuestion> templateQuestions { get; set; }
    }
}