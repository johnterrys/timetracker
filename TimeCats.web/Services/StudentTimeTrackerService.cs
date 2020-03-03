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
            _timeTrackerContext.Users.Add(user);
            _timeTrackerContext.SaveChanges();
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
