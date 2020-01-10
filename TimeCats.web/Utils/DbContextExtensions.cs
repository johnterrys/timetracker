using Microsoft.EntityFrameworkCore;
using TimeCats.Models;

namespace TimeCats.Utils
{
    public static class DbContextExtensions
    {
        private static void ConfigureCoreRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => new
            {
                u.userID
            });
            
            modelBuilder.Entity<Course>().HasKey(c => new
            {
                c.courseID,
                c.instructorID
            });

            modelBuilder.Entity<Group>().HasKey(g => new
            {
                g.groupID
            });

            modelBuilder.Entity<Project>().HasKey(p => new
            {
                p.projectID
            });

            modelBuilder.Entity<TimeCard>().HasKey(tc => new
            {
                tc.timeslotID
            });
            
            modelBuilder.Entity<UserGroup>().HasKey(ug => new
            {
                ug.userID,
                ug.groupID
            });
        }
        
        public static void ConfigureRelationships(this TimeTrackerContext ctx, ModelBuilder modelBuilder)
        {
            ConfigureCoreRelationships(modelBuilder);
            
            // modelBuilder.Entity<AdminEval>().HasKey(eval => new
            // {
            //     eval.evalID
            // });
            //
            // modelBuilder.Entity<AssignEvals>().HasKey(eval => new {
            //     eval.evalTemplateID
            // });
            //
            // modelBuilder.Entity<Dashboard>().HasKey(d => new
            // {
            //     d.courseID,
            //     d.groupID,
            //     d.instructorID,
            //     d.projectID
            // });
            //
            // modelBuilder.Entity<EmptyEval>().HasKey(ee => new
            // {
            //     ee.evalID
            // });
            //
            // modelBuilder.Entity<Eval>().HasKey(eval => new
            // {
            //     eval.evalID
            // });
            //
            // modelBuilder.Entity<EvalColumn>().HasKey(ec => new
            // {
            //     ec.evalID,
            //     ec.originalID
            // });
            //
            // modelBuilder.Entity<EvalResponse>().HasKey(er => new
            // {
            //     er.evalID
            // });
            //
            // modelBuilder.Entity<EvalTemplate>().HasKey(et => new
            // {
            //     et.evalTemplateID
            // });
            //
            // modelBuilder.Entity<EvalTemplateQuestion>().HasKey(etq => new
            // {
            //     etq.evalTemplateQuestionID
            // });
            //
            // modelBuilder.Entity<EvalTemplateQuestionCategory>().HasKey(etqc => new
            // {
            //     etqc.evalTemplateQuestionCategoryID
            // });
            //
            // modelBuilder.Entity<UserCourse>().HasKey(uc => new
            // {
            //     uc.courseID,
            //     uc.userID,
            //     uc.uCourseID
            // });
            //
        }

        public static void SeedData(this TimeTrackerContext ctx, ModelBuilder modelBuilder)
        {
            
        }
    }
}