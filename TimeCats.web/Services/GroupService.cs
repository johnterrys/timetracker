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
                .Include(g => g.UserGroups)
                .ThenInclude(ug => ug.User)
                .FirstOrDefault(g => g.groupID == groupID);
        }

        public Group AddUserToGroup(User user, Group group)
        {
            group.users.Add(user);
            _context.SaveChanges();
            return group;
        }

        public bool IsActiveUserInGroup(int userID, int groupID)
        {
            return _context.UserGroups.Any(ug =>
                ug.userID == userID && ug.groupID == groupID);
        }

        public bool IsUserInOtherGroup(int userID, int groupID)
        {
            var group = GetGroupByID(groupID);
            foreach (UserGroup userGroup in group.UserGroups)
            {
                if (userGroup.userID == userID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Add a User to a Group
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public long JoinGroup(int userID, int groupID)
        {
            try
            {
                var newUser = new UserGroup();

                newUser.userID = userID;
                newUser.groupID = groupID;

                _context.UserGroups.Add(newUser);
                _context.SaveChanges();
            }
            catch (DbUpdateException exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return 0;
            }

            return groupID;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
