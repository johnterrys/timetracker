using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeCats.Models
{
    public class TimeCard
    {
        [Key]
        public int timeslotID { get; set; }
        
        [NotMapped]
        public double hours { get; set; } //TODO: This should probably be a decimal
        
        [Required]
       // [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime timeIn { get; set; }
        
       // [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? timeOut { get; set; }
        
        [NotMapped]
        public bool isEdited { get; set; }
        
        [Required]
        public DateTime createdOn { get; set; }
        
        [Required]
        public int userID { get; set; }
        public User User { get; set; }
        
        [Required]
        public int groupID { get; set; }
        public Group Group { get; set; }
        
        [Required]
        public string description { get; set; }
    }
}