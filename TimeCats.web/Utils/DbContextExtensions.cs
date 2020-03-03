using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Org.BouncyCastle.Utilities.Collections;
using TimeCats.Controllers;
using TimeCats.Models;
using TimeCats.Services;

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

            modelBuilder.Entity<User>()
                .HasIndex(u => u.username)
                .IsUnique();
        }

        public static void ConfigureManyToManyRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCourses)
                .HasForeignKey(uc => uc.userID);

            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.Course)
                .WithMany(c => c.UserCourses)
                .HasForeignKey(uc => uc.courseID);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.UserGroups)
                .HasForeignKey(ug => ug.userID);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany(g => g.UserGroups)
                .HasForeignKey(ug => ug.groupID);
        }

        public static void ConfigureRelationships(this TimeTrackerContext ctx, ModelBuilder modelBuilder)
        {
            ConfigureCoreRelationships(modelBuilder);
            ConfigureManyToManyRelationships(modelBuilder);
        }

        public static void SeedData(this TimeTrackerContext ctx, ModelBuilder modelBuilder)
        {
            var crypto = new CryptographyService();
            var testSalt = crypto.GenerateSalt();
            var testPasswordHash = crypto.CalculateHash(testSalt, "Password!");

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    userID = 1,
                    username = "Admin",
                    firstName = "Adam",
                    lastName = "Admin",
                    type = 'A',
                    isActive = true,
                    password = testPasswordHash,
                    Salt = testSalt
                },
                new User()
                {
                    userID = 2,
                    username = "Instructor",
                    firstName = "Steve",
                    lastName = "Jobs",
                    type = 'I',
                    isActive = true,
                    password = testPasswordHash,
                    Salt = testSalt
                },
                new User()
                {
                    userID = 3,
                    username = "User",
                    firstName = "Normal",
                    lastName = "User",
                    type = 'S',
                    isActive = true,
                    password = testPasswordHash,
                    Salt = testSalt
                });
            modelBuilder.Entity<Group>().HasData(
                new Group()
                {
                    groupID = 1,
                    groupName = "Test Group 1",
                    projectID = 1,
                    isActive = true
                });
            modelBuilder.Entity<Project>().HasData(
                new Project()
                {
                    projectID = 1,
                    projectName = "Test Project 1",
                    description = "This is the first test project",
                    CourseID = 1,
                    isActive = true
                });
            modelBuilder.Entity<Course>().HasData(
                new Course()
                {
                    courseID = 1,
                    courseName = "Test Course",
                    description = "This is a test course for testing.",
                    InstructorId = 2,
                    isActive = true
                });

            modelBuilder.Entity<UserGroup>()
                .HasData(new UserGroup() {userID = 3, groupID = 1});
            modelBuilder.Entity<UserCourse>()
                .HasData(new UserCourse() {userID = 3, courseID = 1});
        }
    }
}
