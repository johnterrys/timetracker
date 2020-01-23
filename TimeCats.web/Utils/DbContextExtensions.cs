using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeCats.Controllers;
using TimeCats.Models;

namespace TimeCats.Utils
{
    public static class DbContextExtensions
    {
        private static void ConfigureCoreRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserGroup>().HasKey(ug => new
            {
                ug.userID,
                ug.groupID
            });
            
            modelBuilder.Entity<UserCourse>().HasKey(uc => new
            {
                uc.userID,
                uc.courseID
            });
        }
        
        public static void ConfigureRelationships(this TimeTrackerContext ctx, ModelBuilder modelBuilder)
        {
            ConfigureCoreRelationships(modelBuilder);
        }

        public static void SeedData(this TimeTrackerContext ctx, ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasData(new List<User>()
                {
                    new User()
                    {
                        userID = 0,
                        username = "Admin",
                        firstName = "Adam",
                        lastName = "Admin",
                        type = 'A',
                        isActive = true,
                        password = HomeController.GenerateHash("Password!")
                    },
                    new User()
                    {
                        userID = 1,
                        username = "instructor",
                        firstName = "Steve",
                        lastName = "Jobs",
                        type = 'I',
                        isActive = true,
                        password = HomeController.GenerateHash("Password!")
                    },
                    new User()
                    {
                        userID = 2,
                        username = "user",
                        firstName = "Normal",
                        lastName = "User",
                        isActive = true,
                        password = HomeController.GenerateHash("Password!"),
                        UserCourses = new List<UserCourse>()
                        {
                            new UserCourse()
                            {
                                isActive = true,
                                userID = 2,
                                courseID = 0
                            }
                        },
                        UserGroups = new List<UserGroup>()
                        {
                            new UserGroup()
                            {
                                userID = 2,
                                groupID = 0
                            }
                        }
                    }
                });

            modelBuilder.Entity<Group>()
                .HasData(new List<Group>()
                {
                    new Group()
                    {
                        groupID = 0,
                        groupName = "Test Group 1",
                        projectID = 0,
                        isActive = true
                    }
                });

            modelBuilder.Entity<Project>()
                .HasData(new List<Project>()
                {
                    new Project()
                    {
                        projectID = 0,
                        projectName = "Test Project 1",
                        description = "This is the first test project",
                        CourseID = 0,
                        isActive = true
                    }
                });

            modelBuilder.Entity<Course>()
                .HasData(new List<Course>()
                {
                    new Course()
                    {
                        courseID = 0,
                        courseName = "Test Course",
                        description = "This is a test course for testing.",
                        instructorName = ctx.Users.FirstOrDefault(u => u.userID == 0)?.firstName,
                        instructorID = ctx.Users.FirstOrDefault(u => u.userID == 0).userID,
                        isActive = true
                    }
                });
        }
    }
}