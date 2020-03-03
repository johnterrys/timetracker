using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TimeCats.Models;

namespace TimeCats.Services
{
    public class GroupService
    {
        private readonly TimeTrackerContext _context;

        public GroupService(TimeTrackerContext context)
        {
            _context = context;
        }

        public Group AddGroup(Group group)
        {
            _context.Groups.Add(group);
            _context.SaveChanges();
            return group;
        }

        public Group GetGroupByID(int groupID)
        {
            return _context.Groups
                .Include(g => g.Project)
                .FirstOrDefault(g => g.groupID == groupID);
        }

        public Group AddUserToGroup(User user, Group group)
        {
            group.users.Add(user);
            _context.SaveChanges();
            return group;
        }
    }
}
