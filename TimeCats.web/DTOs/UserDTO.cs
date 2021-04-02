using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TimeCats.Models;

namespace TimeCats.DTOs
{
    /// <summary>
    ///   Data transfer object for User
    /// </summary>
    public class UserDTO
    {
        [JsonIgnore]
        protected User _user;

        public int userID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string newPassword { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public char type { get; set; }
        public bool isActive { get; set; }
        public byte[] Salt { get; set; }

        public List<GroupDTO> Groups { get; set; }
        public List<CourseDTO> Courses { get; set; }
        public List<TimeCardDTO> timecards { get; set; }

        public UserDTO(User that)
        {
            this._user = that;

            this.userID = that.userID;
            this.username = that.username;
            this.password = that.password;
            this.firstName = that.firstName;
            this.lastName = that.lastName;
            this.type = that.type;
            this.isActive = that.isActive;

            // clone salt
            this.Salt = new byte[that.Salt.Length];
            Array.Copy(that.Salt, this.Salt, that.Salt.Length);

            // not included by default
            this.Groups = null;
            this.Courses = null;
            this.timecards = null;
        }

        public UserDTO WithGroups()
        {
            this.Groups = new List<GroupDTO>();
            foreach (UserGroup userGroup in _user.UserGroups)
            {
                this.Groups.Add(new GroupDTO(userGroup.Group));
            }

            return this;
        }

        public UserDTO WithCourses()
        {
            this.Courses = new List<CourseDTO>();
            foreach (Course course in _user.Courses)
            {
                this.Courses.Add(new CourseDTO(course));
            }

            return this;
        }

        public UserDTO WithTimeCards(int gID)
        {
            this.timecards = new List<TimeCardDTO>();
            foreach (TimeCard timecard in _user.timecards)
            {
                if (timecard.groupID == gID)
                {
                    this.timecards.Add(new TimeCardDTO(timecard));
                }
            }

            return this;
        }
    }
}
