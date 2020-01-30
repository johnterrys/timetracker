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

        public IEnumerable<Course> GetCoursesByUser(User user)
        {
            var courses = _context.Courses
                .Where(c => c.InstructorId == user.userID);

            return courses;
        }

        public IEnumerable<Course> GetCourses()
        {
            var courses = _context.Courses
                .Include(c => c.Instructor).ToList();

            return courses;
        }
    }
}