using System;
using System.Collections.Generic;
using System.Linq;
using TimeCats.Models;

namespace TimeCats.Services
{
    public class StudentTimeTrackerService
    {
        private readonly TimeTrackerContext _timeTrackerContext;

        public StudentTimeTrackerService(TimeTrackerContext studentCtx)
        {
            _timeTrackerContext = studentCtx;
        }

        public void AddUser(User user)
        {
            var crypto = new CryptographyService();
            var salt = crypto.GenerateSalt();
            user.password = crypto.CalculateHash(salt, user.password);
            user.Salt = salt;
            user.isActive = true;
            // This is a temp fix for users not being able to register (fix this properly by changing the id type to a GUID)
            user.userID = _timeTrackerContext.Users.Max(user => user.userID) + 1;
            _timeTrackerContext.Users.Add(user);
            _timeTrackerContext.SaveChanges();
            // Test doing a commit.
        }

        public User GetUserByID(int userID)
        {
            return _timeTrackerContext.Users.
                FirstOrDefault(u => u.userID == userID);
        }

        public User GetUserByUsername(string username)
        {
            var user = _timeTrackerContext.Users.FirstOrDefault(u => u.username.ToLower().Equals(username.ToLower()));
            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            return _timeTrackerContext.Users;
        }

        public ICollection<Dashboard> GetDashboardsForUser()
        {
            throw new NotImplementedException();
        }

        

        public int AddCourse(Course course)
        {
            var c = _timeTrackerContext.Courses.Add(course);
            _timeTrackerContext.SaveChanges();

            return c.Entity.courseID;
        }
    }
}
