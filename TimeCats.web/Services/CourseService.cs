using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TimeCats.Models;

namespace TimeCats.Services
{
    public class CourseService
    {
        private readonly TimeTrackerContext _context;

        public CourseService(TimeTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a course to the database.
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public void CreateCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();  
        }

        /// <summary>
        /// Adds a course to the database
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public Course AddCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
            
            return course;
        }

        /// <summary>
        /// Get a single Course from a CourseID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Course GetCourse(int id)
        {
            return _context.Courses
                        .Include(c => c.Instructor)
                        .FirstOrDefault(c => c.courseID == id);
        }

        /// <summary>
        /// Save a course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public bool SaveCourse(Course course)
        {
            try
            {
                _context.Courses.Update(course);
                _context.SaveChanges();
            }
            catch(DbUpdateException exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a list of Courses from a UserID
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public IEnumerable<Course> GetCoursesByUser(User user)
        {
            var courses = _context.Courses
                               .Where(c => c.InstructorId == user.userID);

            return courses;
        }

        /// <summary>
        /// Get a list of all Courses
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Course> GetCourses()
        {
            var courses = _context.Courses
                                .Include(c => c.Instructor)
                                .ToList();

            return courses;
        }

        /// <summary>
        /// Get a list of Inactive Users from a CourseID
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<User> GetInactiveUsersForCourse(int courseID)
        {
            var courses = _context.UserCourses
                            .Where(uc => uc.courseID == courseID && uc.isActive == false)
                            .Select(uc => uc.userID)
                            .ToList();

            var names = _context.Users
                            .Where(u => courses.Contains(u.userID))
                            .ToList();
            return names;
        }

        /// <summary>
        /// Get a Course from a GroupID
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public int GetCourseForGroup(int groupID)
        {
            var groups = _context.Groups
                            .Where(g => g.groupID == groupID)
                            .Select(g => g.projectID)
                            .ToList();

            var projects = _context.Projects
                            .Where(p => groups.Contains(p.projectID))
                            .Select(p => p.CourseID)
                            .ToList();

            var courses = _context.Courses
                            .Where(c => projects.Contains(c.courseID))
                            .Select(c => c.courseID)
                            .FirstOrDefault();

            return courses;
        }


        /// <summary>
        /// Add an Inactive User to a Course
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public int RequestJoinCourse(int userId, int courseId)
        {
            _context.UserCourses.Add(new UserCourse
            {
                userID = userId,
                courseID = courseId, 
                isActive = false
            });

            return _context.SaveChanges();
        }


        /// <summary>
        /// Activate a User in a Course
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool JoinCourse(int courseID, int userID)
        {
            try
            {
                var newuser = new UserCourse();

                newuser.userID = userID;
                newuser.courseID = courseID;
                newuser.isActive = true;

                _context.UserCourses.Add(newuser);
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
        /// Deactivate a User in a Course
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool LeaveCourse(int courseID, int userID)
        {
            try
            {
                var names = _context.UserCourses
                                .FirstOrDefault(u => u.userID == userID && u.courseID == courseID);

                names.isActive = false;

                _context.UserCourses.Update(names);
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
        /// Delete a User in a Course
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public bool DeleteUserCourse(int userID, int courseID)
        {
            try
            {
                var name = _context.UserCourses
                    .FirstOrDefault(u => u.userID == userID && u.courseID == courseID);

                _context.Remove(name);
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
        /// Get an Instructor from a CourseID
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public int GetInstructorForCourse(int courseID)
        {
            return _context.Courses
                        .Where(u => u.courseID == courseID)
                        .Select(u => u.InstructorId)
                        .FirstOrDefault();
        }


        /// <summary>
        /// Save a User in a Course
        /// </summary>
        /// <param name="userCourse"></param>
        /// <returns></returns>
        public bool SaveUserInCourse(UserCourse userCourse)
        {
            try
            {
                userCourse.isActive = true;

                _context.UserCourses.Update(userCourse);
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
        /// Delete a User from a Course
        /// </summary>
        /// <param name="course"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool DeleteFromCourse(int course, int user)
        {
            try
            {
                var names = _context.UserCourses
                               .FirstOrDefault(u => u.courseID == course && u.userID == user);

                _context.UserCourses.Remove(names);
                _context.SaveChanges();
            }
            catch (DbUpdateException exc)
            {
                Console.WriteLine(exc.InnerException.Message);
                return false;
            }

            return true;
        }


    }
}