using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TimeCats.Migrations
{
    public partial class InitalMigrationWithSeeds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    Salt = table.Column<byte[]>(nullable: false),
                    firstName = table.Column<string>(nullable: false),
                    lastName = table.Column<string>(nullable: false),
                    type = table.Column<char>(nullable: false),
                    isActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userID);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    courseID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    courseName = table.Column<string>(nullable: false),
                    InstructorId = table.Column<int>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.courseID);
                    table.ForeignKey(
                        name: "FK_Courses_Users_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    projectID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    projectName = table.Column<string>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    description = table.Column<string>(nullable: false),
                    CourseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.projectID);
                    table.ForeignKey(
                        name: "FK_Projects_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "courseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserCourses",
                columns: table => new
                {
                    userID = table.Column<int>(nullable: false),
                    courseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourses", x => new { x.userID, x.courseID });
                    table.ForeignKey(
                        name: "FK_UserCourses_Courses_courseID",
                        column: x => x.courseID,
                        principalTable: "Courses",
                        principalColumn: "courseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCourses_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    groupID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    groupName = table.Column<string>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    projectID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.groupID);
                    table.ForeignKey(
                        name: "FK_Groups_Projects_projectID",
                        column: x => x.projectID,
                        principalTable: "Projects",
                        principalColumn: "projectID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeCards",
                columns: table => new
                {
                    timeslotID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    timeIn = table.Column<string>(nullable: false),
                    timeOut = table.Column<string>(nullable: false),
                    createdOn = table.Column<string>(nullable: false),
                    userID = table.Column<int>(nullable: false),
                    groupID = table.Column<int>(nullable: false),
                    description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeCards", x => x.timeslotID);
                    table.ForeignKey(
                        name: "FK_TimeCards_Groups_groupID",
                        column: x => x.groupID,
                        principalTable: "Groups",
                        principalColumn: "groupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeCards_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    userID = table.Column<int>(nullable: false),
                    groupID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => new { x.userID, x.groupID });
                    table.ForeignKey(
                        name: "FK_UserGroups_Groups_groupID",
                        column: x => x.groupID,
                        principalTable: "Groups",
                        principalColumn: "groupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "userID", "Salt", "firstName", "isActive", "lastName", "password", "type", "username" },
                values: new object[,]
                {
                    { 1, new byte[] { 190, 176, 135, 224, 169, 56, 102, 7, 176, 216, 51, 210, 173, 9, 127, 133, 175, 162, 0, 106, 109, 47, 104, 193, 19, 15, 16, 119, 247, 150, 198, 151 }, "Adam", true, "Admin", "bSr2t3bUhq39QdFZvwPwG1diG4sRMS92KJz0wzcRQqE=", 'A', "Admin" },
                    { 2, new byte[] { 190, 176, 135, 224, 169, 56, 102, 7, 176, 216, 51, 210, 173, 9, 127, 133, 175, 162, 0, 106, 109, 47, 104, 193, 19, 15, 16, 119, 247, 150, 198, 151 }, "Steve", true, "Jobs", "bSr2t3bUhq39QdFZvwPwG1diG4sRMS92KJz0wzcRQqE=", 'I', "Instructor" },
                    { 3, new byte[] { 190, 176, 135, 224, 169, 56, 102, 7, 176, 216, 51, 210, 173, 9, 127, 133, 175, 162, 0, 106, 109, 47, 104, 193, 19, 15, 16, 119, 247, 150, 198, 151 }, "Normal", true, "User", "bSr2t3bUhq39QdFZvwPwG1diG4sRMS92KJz0wzcRQqE=", 'S', "User" }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "courseID", "InstructorId", "courseName", "description", "isActive" },
                values: new object[] { 1, 2, "Test Course", "This is a test course for testing.", true });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "projectID", "CourseID", "description", "isActive", "projectName" },
                values: new object[] { 1, 1, "This is the first test project", true, "Test Project 1" });

            migrationBuilder.InsertData(
                table: "UserCourses",
                columns: new[] { "userID", "courseID" },
                values: new object[] { 3, 1 });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "groupID", "groupName", "isActive", "projectID" },
                values: new object[] { 1, "Test Group 1", true, 1 });

            migrationBuilder.InsertData(
                table: "UserGroups",
                columns: new[] { "userID", "groupID" },
                values: new object[] { 3, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_InstructorId",
                table: "Courses",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_projectID",
                table: "Groups",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CourseID",
                table: "Projects",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_TimeCards_groupID",
                table: "TimeCards",
                column: "groupID");

            migrationBuilder.CreateIndex(
                name: "IX_TimeCards_userID",
                table: "TimeCards",
                column: "userID");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourses_courseID",
                table: "UserCourses",
                column: "courseID");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_groupID",
                table: "UserGroups",
                column: "groupID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_username",
                table: "Users",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeCards");

            migrationBuilder.DropTable(
                name: "UserCourses");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
