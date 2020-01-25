using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TimeCats.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    courseID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    courseName = table.Column<string>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    description = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.courseID);
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
                name: "Users",
                columns: table => new
                {
                    userID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(nullable: false),
                    password = table.Column<string>(nullable: false),
                    firstName = table.Column<string>(nullable: false),
                    lastName = table.Column<string>(nullable: false),
                    type = table.Column<char>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    groupID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userID);
                    table.ForeignKey(
                        name: "FK_Users_Groups_groupID",
                        column: x => x.groupID,
                        principalTable: "Groups",
                        principalColumn: "groupID",
                        onDelete: ReferentialAction.Restrict);
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
                name: "UserCourse",
                columns: table => new
                {
                    userID = table.Column<int>(nullable: false),
                    courseID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourse", x => new { x.userID, x.courseID });
                    table.ForeignKey(
                        name: "FK_UserCourse_Courses_courseID",
                        column: x => x.courseID,
                        principalTable: "Courses",
                        principalColumn: "courseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCourse_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroup",
                columns: table => new
                {
                    userID = table.Column<int>(nullable: false),
                    groupID = table.Column<int>(nullable: false),
                    projectID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroup", x => new { x.userID, x.groupID });
                    table.ForeignKey(
                        name: "FK_UserGroup_Groups_groupID",
                        column: x => x.groupID,
                        principalTable: "Groups",
                        principalColumn: "groupID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroup_Projects_projectID",
                        column: x => x.projectID,
                        principalTable: "Projects",
                        principalColumn: "projectID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroup_Users_userID",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_UserCourse_courseID",
                table: "UserCourse",
                column: "courseID");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_groupID",
                table: "UserGroup",
                column: "groupID");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroup_projectID",
                table: "UserGroup",
                column: "projectID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_groupID",
                table: "Users",
                column: "groupID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeCards");

            migrationBuilder.DropTable(
                name: "UserCourse");

            migrationBuilder.DropTable(
                name: "UserGroup");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Courses");
        }
    }
}
