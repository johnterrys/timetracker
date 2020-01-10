using System.Collections.Generic;

namespace TimeCats.Models
{
    public class User
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string newPassword { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public char type { get; set; }
        public bool isActive { get; set; }
        public List<TimeCard> timecards { get; set; }
    }
}