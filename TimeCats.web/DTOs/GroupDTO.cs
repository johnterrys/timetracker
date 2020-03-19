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

        // still need to add UserDTO
        //public List<UserDTO> users { get; set; }

        public GroupDTO(Group that)
        {
            this.groupID = that.groupID;
            this.groupName = that.groupName;
            this.isActive = that.isActive;
            this.evalID = that.evalID;

            // not included by default
            this.Project = null;
        }

        public GroupDTO WithProject()
        {
            this.Project = new ProjectDTO(_group.Project);
            return this;
        }
    }
}
