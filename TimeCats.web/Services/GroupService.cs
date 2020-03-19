using System;
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

        public long CreateGroup(Group group)
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
        /// Get a Group from a GroupID
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public Group GetGroup(int groupID)
        {
            return _context.Groups
                   .FirstOrDefault(g => g.groupID == groupID);
        }


        /// <summary>
        /// Save a Group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public bool SaveGroup(Group group)
        {
            try
            {
                _context.Groups.Update(group);
                _context.SaveChanges();
            }
            catch (DbUpdateException exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return false;
            }

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

                //names.isActive = false;

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

                //names.isActive = true;

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

        public bool IsUserInOtherGroup(int userID, int groupID)
        {
            var groupNumber = _context.Groups
                            .FirstOrDefault(u => u.groupID == groupID);

            var names = (from u in _context.Users
                         join ug in _context.UserGroups on u.userID equals ug.userID
                         join g in _context.Groups on ug.groupID equals g.groupID
                         join p in _context.Projects on g.projectID equals p.projectID
                         where (u.userID == userID && g.projectID == groupID)
                         select new { u.userID })
                        .Any();
            return names;
        }

        //    var isInGroup = false;
        //    using (var conn = new MySqlConnection(ConnString.ToString()))
        //    {
        //        conn.Open();
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            //SQL and Parameters
        //            cmd.CommandText = " Select u.userID, u.firstName, u.lastName, ug.groupID From users u " +
        //                              "Inner Join uGroups ug On u.userID = ug.userID Inner Join cs4450.groups g On ug.groupID = g.groupID Where u.userID = @userID " +
        //                              "And g.projectID = 	(SELECT projectID FROM cs4450.groups WHERE groupID = @groupID) " +
        //                              "And ug.isActive = 1";
        //            cmd.Parameters.AddWithValue("@userID", userID);
        //            cmd.Parameters.AddWithValue("@groupID", groupID);

        //            using (var reader = cmd.ExecuteReader())
        //            {
        //                //Runs once per record retrieved
        //                while (reader.Read())
        //                    if (!isInGroup)
        //                        isInGroup = true;
        //            }
        //        }
        //    }

        //    return isInGroup;
        //}


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
