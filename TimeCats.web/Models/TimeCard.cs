﻿using System.ComponentModel.DataAnnotations;
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
        public string timeIn { get; set; } //TODO: Convert to DATETIME
        
        [Required]
        public string timeOut { get; set; } //TODO: Convert to DATETIME
        
        [NotMapped]
        public bool isEdited { get; set; }
        
        [Required]
        public string createdOn { get; set; } //TODO: Convert this to a DATETIME2
        
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