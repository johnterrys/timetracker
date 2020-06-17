using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TimeCats.Models;

namespace TimeCats.DTOs
{
    /// <summary>
    ///   Data transfer object for TimeCard
    /// </summary>
    public class TimeCardDTO
    {
        [JsonIgnore]
        protected TimeCard _timeCard;

        public int timeslotID { get; set; }
        public double hours { get; set; }
        public string timeIn { get; set; }
        public string timeOut { get; set; }
        public bool isEdited { get; set; }
        public DateTime createdOn { get; set; }
        public int userID { get; set; }
        public int groupID { get; set; }
        public string description { get; set; }

        public UserDTO User { get; set; }
        public GroupDTO Group { get; set; }

        public TimeCardDTO(TimeCard that)
        {
            this._timeCard = that;

            this.timeslotID = that.timeslotID;
            this.hours = that.hours;
            this.timeIn = that.timeIn.ToString("MM/dd/yyyy hh:mm tt");
            this.timeOut = that.timeOut?.ToString("MM/dd/yyyy hh:mm tt");
            this.isEdited = that.isEdited;
            this.createdOn = that.createdOn;
            this.userID = that.userID;
            this.groupID = that.groupID;
            this.description = that.description;

            // not included by default
            this.User = null;
            this.Group = null;
        }

        public TimeCardDTO WithUser()
        {
            this.User = new UserDTO(_timeCard.User);

            return this;
        }

        public TimeCardDTO WithGroup()
        {
            this.Group = new GroupDTO(_timeCard.Group);

            return this;
        }
    }
}
