namespace TimeCats.Models
{
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
        public int instructorID { get; set; }
        public string instructorName { get; set; }

        public string templateName { get; set; }
    }
}