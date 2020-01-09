using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace time_sucks.Models
{
    public class Eval
    {
        public int evalID { get; set; }
        public int evalTemplateID { get; set; }
        public int groupID { get; set; }
        public int userID { get; set; }

        public int number { get; set; }
        public bool isComplete { get; set; }

        public List<EvalTemplateQuestionCategory> categories { get; set; }
        public List<EvalTemplateQuestion> templateQuestions { get; set; }
        public List<EvalResponse> responses { get; set; }
        public List<User> users { get; set; }
        public List<EvalColumn> evals { get; set; }

        //  States for Mr. Peterson
        public Dictionary<int, int> columnSums { get; set;}
        public Dictionary<int, double> userAvgerage { get; set; }
    }
}
