using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TimeCats.Models;
using TimeCats.Utils;

namespace TimeCats
{
    public class TimeTrackerContext : DbContext
    {
        // ORIGINAL TABLES
        public DbSet<Course> Courses { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TimeCard> TimeCards { get; set; }
        
        // JOINING TABLES
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        // public DbSet<Eval> Evals { get; set; }
        // public DbSet<EvalResponse> EvalResponses { get; set; }
        // public DbSet<EvalTemplate> EvalTemplates { get; set; }
        // public DbSet<EvalTemplateQuestion> EvalTemplateQuestions { get; set; }
        // public DbSet<EvalTemplateQuestionCategory> EvalTemplateQuestionCategories { get; set; }

        public TimeTrackerContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.ConfigureRelationships(modelBuilder);
            this.SeedData(modelBuilder);
        }
    }
}