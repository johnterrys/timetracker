using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TimeCats.Models;
using TimeCats.Utils;

namespace TimeCats
{
    public class TimeTrackerContext : DbContext
    {
        public DbSet<AdminEval> AdminEvals { get; set; }
        public DbSet<AssignEvals> AssignmentEvals { get; set; } // TODO: is this actually an assignment?
        public DbSet<Course> Courses { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<Eval> Evals { get; set; }
        public DbSet<EvalColumn> EvalColumns { get; set; }
        public DbSet<EvalResponse> EvalResponses { get; set; }
        public DbSet<EvalTemplate> EvalTemplates { get; set; }
        public DbSet<EvalTemplateQuestion> EvalTemplateQuestions { get; set; }
        public DbSet<EvalTemplateQuestionCategory> EvalTemplateQuestionCategories { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TimeCard> TimeCards { get; set; }
        public DbSet<uCourse> UCourses { get; set; }
        public DbSet<uGroups> UGroups { get; set; }
        public DbSet<User> Users { get; set; }
        
        public TimeTrackerContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            this.ConfigureRelationships(modelBuilder);
        }
    }
}