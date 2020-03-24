using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TimeCats.Models;

namespace TimeCats.DTOs
{
    public class GroupDTO
    {
        [JsonIgnore]
        protected Group _group;

        public int groupID { get; set; }
        public string groupName { get; set; }
        public bool isActive { get; set; }
        public int evalID { get; set; }

        public ProjectDTO Project { get; set; }
        public List<UserDTO> users { get; set; }

        public GroupDTO(Group that)
        {
            this._group = that;

            this.groupID = that.groupID;
            this.groupName = that.groupName;
            this.isActive = that.isActive;
            this.evalID = that.evalID;

            // not included by default
            this.Project = null;
            this.users = null;
        }

        public GroupDTO WithProject()
        {
            this.Project = new ProjectDTO(_group.Project);
            return this;
        }

        public GroupDTO WithUsers()
        {
            this.users = new List<UserDTO>();
            foreach (UserGroup userGroup in _group.UserGroups)
            {
                this.users.Add(new UserDTO(userGroup.User));
            }

            return this;
        }
    }
}
