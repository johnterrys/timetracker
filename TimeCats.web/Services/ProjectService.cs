using System.Collections.Generic;
using System.Linq;
using TimeCats.Models;

namespace TimeCats.Services
{
    public class ProjectService
    {
        private readonly TimeTrackerContext _context;

        public ProjectService(TimeTrackerContext context)
        {
            _context = context;
        }

        public Project AddProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return project;
        }

        public Project GetProjectById(int projectId)
        {
            return _context.Projects.FirstOrDefault(p => p.projectID == projectId);
        }

        public IEnumerable<Project> GetProjects()
        {
            return _context.Projects;
        }

        public IEnumerable<Project> GetProjectsByCourseId(int courseId)
        {
            return _context.Projects
                .Where(p => p.CourseID == courseId);
        }
    }
}