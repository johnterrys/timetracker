using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Add a Project to the Database
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public Project AddProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();

            return project;
        }

        /// <summary>
        /// Get a single Project from a ProjectID
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public Project GetProject(int projectID)
        {
            return _context.Projects
                    .Where(p => p.projectID == projectID)
                    .FirstOrDefault();
        }


        /// <summary>
        /// Get a single Project from a ProjectID
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public Project GetProjectById(int projectId)
        {
            return _context.Projects
                .Include(p => p.groups)
                .FirstOrDefault(p => p.projectID == projectId);
        }


        /// <summary>
        /// Get a list of Projects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Project> GetProjects()
        {
            return _context.Projects;
        }


        /// <summary>
        /// Get a list of Projects from a CourseID
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IEnumerable<Project> GetProjectsByCourseId(int courseId)
        {
            return _context.Projects
                .Where(p => p.CourseID == courseId);
        }


        /// <summary>
        /// Save a Project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public bool SaveProject(Project project)
        {
            try
            {
                _context.Projects.Update(project);
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
