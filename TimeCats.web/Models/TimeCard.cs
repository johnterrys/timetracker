namespace TimeCats.Models
{
    public class TimeCard
    {
        public int timeslotID { get; set; }
        public double hours { get; set; }
        public string timeIn { get; set; }
        public string timeOut { get; set; }
        public bool isEdited { get; set; }
        public string createdOn { get; set; }
        public int userID { get; set; }
        public int groupID { get; set; }
        public string description { get; set; }
    }
}