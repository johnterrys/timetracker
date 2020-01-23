using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Org.BouncyCastle.Utilities.Collections;
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
            var users = new List<User>() {
                new User()
                {
                    userID = 1,
                    username = "Admin",
                    firstName = "Adam",
                    lastName = "Admin",
                    type = 'A',
                    isActive = true,
                    password = HomeController.GenerateHash("Password!")
                },
                new User()
                {
                    userID = 2,
                    username = "Instructor",
                    firstName = "Steve",
                    lastName = "Jobs",
                    type = 'I',
                    isActive = true,
                    password = HomeController.GenerateHash("Password!")
                },
                new User()
                {
                    userID = 3,
                    username = "User",
                    firstName = "Normal",
                    lastName = "User",
                    type = 'S',
                    isActive = true,
                    password = HomeController.GenerateHash("Password!")
                }
            };
            var userCourses = new List<UserCourse>() {
                new UserCourse()
                {
                    isActive = true,
                    userID = 2,
                    courseID = 0
                }
            };
            var userGroups = new List<UserGroup>() {
                new UserGroup()
                {
                    userID = 2,
                    groupID = 0
                }
            };
            var groups = new List<Group>() {
                new Group()
                {
                    groupID = 1,
                    groupName = "Test Group 1",
                    projectID = 1,
                    isActive = true
                }
            };
            var projects = new List<Project>() {
                new Project()
                {
                    projectID = 1,
                    projectName = "Test Project 1",
                    description = "This is the first test project",
                    CourseID = 1,
                    isActive = true
                }
            };
            var courses = new List<Course>() {
                new Course()
                {
                    courseID = 1,
                    courseName = "Test Course",
                    description = "This is a test course for testing.",
                    instructorName = users[1].firstName,
                    instructorID = users[1].userID,
                    isActive = true
                }
            };
            
            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<UserCourse>().HasData(userCourses);
            modelBuilder.Entity<UserGroup>().HasData(userGroups);
            modelBuilder.Entity<Group>().HasData(groups);
            modelBuilder.Entity<Project>().HasData(projects);
            modelBuilder.Entity<Course>().HasData(courses);
        }
    }
}