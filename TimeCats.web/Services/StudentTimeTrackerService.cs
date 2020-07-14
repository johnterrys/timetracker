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

        public ICollection<Dashboard> GetDashboardsForUser(int userID)
        {
            var dashboard = new List<Dashboard>();
            var userGroups = new List<Group>();

            foreach (Group group in _timeTrackerContext.Groups.ToList())
            {
                foreach (UserGroup usergroup in _timeTrackerContext.UserGroups.ToList())
                {
                    if ( (!userGroups.Contains(usergroup.Group)) && (userID == usergroup.userID) )
                    {
                        userGroups.Add(usergroup.Group);
                    }
                }
            }




            foreach (Group group in userGroups)
            {
                var projectname = _timeTrackerContext.Projects.Where(p => p.projectID == group.projectID).Select(p => new { p.projectName }).ToList()[0].projectName;
                var courseid = _timeTrackerContext.Projects.Where(p => p.projectID == group.projectID).Select(p => new { p.CourseID }).ToList()[0].CourseID;
                var coursename = _timeTrackerContext.Courses.Where(c => c.courseID == courseid).Select(c => new { c.courseName }).ToList()[0].courseName;
                var instructorid = _timeTrackerContext.Courses.Where(c => c.courseID == courseid).Select(c => new { c.InstructorId }).ToList()[0].InstructorId;
                var instructorname = _timeTrackerContext.Users.Where(u => u.userID == instructorid).Select(u => new { fullname = u.firstName + " " + u.lastName }).ToList()[0].fullname;

                dashboard.Add(new Dashboard
                {
                    groupID = group.groupID,
                    groupName = group.groupName,
                    projectID = group.projectID,
                    projectName = projectname,
                    courseID = courseid,
                    courseName = coursename,
                    instructorId = instructorid,
                    instructorName = instructorname
                });
            }

            //throw new NotImplementedException();

            return dashboard;
        }

        

        public int AddCourse(Course course)
        {
            var c = _timeTrackerContext.Courses.Add(course);
            _timeTrackerContext.SaveChanges();

            return c.Entity.courseID;
        }
    }
}
