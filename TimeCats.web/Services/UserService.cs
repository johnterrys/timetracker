using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TimeCats.Models;

namespace TimeCats.Services
{
    public class UserService
    {
        private TimeTrackerContext _context;

        public UserService(TimeTrackerContext context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        //Admin version doesn't require the current password to be passed
        public bool ChangePasswordA(User user)
        {
            user.password = user.newPassword;
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }

        //Normal version requires current password to be passed
        public bool ChangePassword(User user)
        {
            user.password = user.newPassword;
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }

        //Normal version doesn't save type or isActive
        public bool ChangeUser(User user)
        {
            var entry = _context.Users.First(u => u.userID == user.userID);

            _context.Entry(entry).CurrentValues.SetValues(user);
            _context.SaveChanges();
            return true;
        }



        public IEnumerable<User> GetUsersForCourse(int courseId)
        {
            var users = _context.UserCourses
                .Where(uc => uc.courseID == courseId)
                .Select(uc => uc.User);
            return users;
        }

        public bool UserIsInCourse(int courseID, int userID)
        {
            return _context.UserCourses
                        .Any(u => u.courseID == courseID && u.userID == userID);
        }

        public User GetUser(string username)
        {
            return _context.Users
                   .Where(u => u.username == username)
                   .FirstOrDefault();
        }


        public User GetUser(string username, string password)
        {
            return _context.Users
                    .Where(u => u.username == username && u.password == password)
                    .FirstOrDefault();
        }

        public bool IsUserInGroup(int userID, int groupID)
        {
            return _context.UserGroups
                .Any(u => u.userID == userID && u.groupID == groupID);
        }


        public bool IsActiveUserInGroup(int userID, int groupID)
        {
            return _context.UserGroups
                .Any(u => u.userID == userID && u.groupID == groupID);
        }


        public User GetUserByID(int ID)
        {
            return _context.Users
                    .Where(u => u.userID == ID)
                    .FirstOrDefault();
        }


        // Return a list of users within a group
        public List<User> GetUsersForGroup(int groupID)
        {
            var userGroups = _context.UserGroups
                                .Where(ug => ug.groupID == groupID)
                                .Select(ug => ug.userID)
                                .ToList();

            var names = _context.Users
                            .Where(u => userGroups.Contains(u.userID))
                            .Select(u => new User
                            {
                                userID = u.userID,
                                firstName = u.firstName,
                                lastName = u.lastName
                            })
                            .ToList();

            return names;
        }
    }
}
