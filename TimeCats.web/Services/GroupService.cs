using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TimeCats.Models;
using MySql.Data.MySqlClient;

namespace TimeCats.Services
{
    public class GroupService
    {
        private readonly TimeTrackerContext _context;

        public GroupService(TimeTrackerContext context)
        {
            _context = context;
        }

        public Group GetGroup(int groupID)
        {
            return _context.Groups
                .Include(g => g.Project)
                .Include(g => g.UserGroups)
                .ThenInclude(ug => ug.User)
                .ThenInclude(u => u.timecards)
                .FirstOrDefault(g => g.groupID == groupID);
        }

        public bool IsUserInOtherGroup(int userID, int groupID)
        {
            var group = GetGroup(groupID);
            foreach (UserGroup userGroup in group.UserGroups)
            {
                if (userGroup.userID == userID)
                {
                    return true;
                }
            }

            return false;
        }

        public int CreateGroup(Group group)
        {
            try
            {
                _context.Groups.Add(group);
                _context.SaveChanges();
            }
            catch (DbUpdateException exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return 0;
            }

            return group.groupID;
        }

        /// <summary>
        /// Save a Group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool SaveGroup(Group groupData)
        {
            var group = GetGroup(groupData.groupID);
            group.groupName = groupData.groupName;
            group.isActive = groupData.isActive;
            group.evalID = groupData.evalID;

            _context.SaveChanges();

            return true;
        }

        public bool DeleteFromGroup(int userID, int groupID)
        {
            try
            {
                var names = _context.UserGroups
                    .FirstOrDefault(u => u.userID == userID && u.groupID == groupID);

                _context.UserGroups.Remove(names);
                _context.SaveChanges();
            }
            catch (DbUpdateException exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Remove a User from a Group
        /// </summary>
        /// <param name="user"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool LeaveGroup(int user, int group)
        {
            try
            {
                var names = _context.UserGroups
                    .FirstOrDefault(u => u.userID == user && u.groupID == group);

                _context.UserGroups.Remove(names);
                _context.SaveChanges();
            }
            catch (DbUpdateException exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return false;
            }

            return true;
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

        /// <summary>
        /// Add a User to a Group
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool ReJoinGroup(int userID, int groupID)
        {
            try
            {
                var names = _context.UserGroups
                    .FirstOrDefault(u => u.userID == userID && u.groupID == groupID);

                _context.UserGroups.Update(names);
                _context.SaveChanges();
            }
            catch (DbUpdateException exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return false;
            }

            return true;
        }

        public bool IsUserInGroupForProject(int userID, int projectID)
        {
            var project = _context.Projects
                            .Where(p => p.projectID == projectID)
                            .Select(p => p.groups)
                            .ToList();

            var names = (from u in _context.Users
                         join ug in _context.UserGroups on u.userID equals ug.userID
                         join g in _context.Groups on ug.groupID equals g.groupID
                         where (u.userID == userID && g.projectID == projectID)
                         select new { u.userID })
                        .Any();

            return names;
        }
    }
}
