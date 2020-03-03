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

        public Course AddCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
            return course;
        }

        public Course GetCourse(int id)
        {
            return _context.Courses
                .Include(c => c.Instructor)
                .FirstOrDefault(c => c.courseID == id);
        }

        public List<Course> GetCoursesByUser(User user)
        {
            var courses = _context.Courses
                .Where(c => _context.UserCourses.Any(
                       uc => uc.courseID == c.courseID &&
                             uc.userID == user.userID))
                .ToList();

            return courses;
        }

        public List<Course> GetCourses()
        {
            var courses = _context.Courses
                .Include(c => c.Instructor)
                .ToList();

            return courses;
        }
    }
}
