namespace TimeCats.Models
{
    public class TimeCard
    {
        public int timeslotID { get; set; }
        
        public double hours { get; set; } //TODO: This should probably be a decimal
        
        public string timeIn { get; set; } //TODO: Convert to DATETIME
        
        public string timeOut { get; set; } //TODO: Convert to DATETIME
        
        public bool isEdited { get; set; }
        
        public string createdOn { get; set; } //TODO: Convert this to a DATETIME2
        
        public int userID { get; set; }
        public User User { get; set; }
        
        public int groupID { get; set; }
        public Group Group { get; set; }
        
        public string description { get; set; }
    }
}