namespace TimeCats.Models
{
    //I still have no idea what this is actually used for or how it really differs from a normal Eval
    //Tags will need to be added and a database migration will need to be done like other Models (Look at Eval.cs for an example)
    public class AdminEval
    {
        public int evalID { get; set; }
        public int evalTemplateID { get; set; }
        public int groupID { get; set; }
        public int userID { get; set; }
        public int number { get; set; }
        public bool isComplete { get; set; }

        public string usersName { get; set; }
        public string groupName { get; set; }
        public int projectID { get; set; }
        public string projectName { get; set; }
        public int courseID { get; set; }
        public string courseName { get; set; }
        public int instructorId { get; set; }
        public string instructorName { get; set; }

        public string templateName { get; set; }
    }
}