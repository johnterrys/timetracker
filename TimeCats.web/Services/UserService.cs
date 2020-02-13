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

        public IEnumerable<User> GetUsersForCourse(int courseId)
        {
            var users = _context.UserCourses
                .Where(uc => uc.courseID == courseId)
                .Select(uc => uc.User);
            return users;
        }
    }
}