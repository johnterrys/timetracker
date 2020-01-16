using System.Linq;
using TimeCats.Models;

namespace TimeCats
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
            _timeTrackerContext.Users.Add(user);
            _timeTrackerContext.SaveChanges();
        }

        public User GetUserByUsername(string username)
        {
            var user = _timeTrackerContext.Users.FirstOrDefault(u => u.username.Equals(username));
            return user;
        }

        public User GetUserWithPasswordHash(string username, string hash)
        {
            var user = _timeTrackerContext.Users
                .FirstOrDefault(u => u.username.Equals(username) && 
                                     u.password.Equals(hash));
            return user;
        }
    }
}