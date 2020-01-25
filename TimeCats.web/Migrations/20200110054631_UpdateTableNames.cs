using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeCats.Migrations
{
    public partial class UpdateTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCourse_Courses_courseID",
                table: "UserCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourse_Users_userID",
                table: "UserCourse");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroup_Groups_groupID",
                table: "UserGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroup_Projects_projectID",
                table: "UserGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroup_Users_userID",
                table: "UserGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroup",
                table: "UserGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCourse",
                table: "UserCourse");

            migrationBuilder.RenameTable(
                name: "UserGroup",
                newName: "UserGroups");

            migrationBuilder.RenameTable(
                name: "UserCourse",
                newName: "UserCourses");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroup_projectID",
                table: "UserGroups",
                newName: "IX_UserGroups_projectID");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroup_groupID",
                table: "UserGroups",
                newName: "IX_UserGroups_groupID");

            migrationBuilder.RenameIndex(
                name: "IX_UserCourse_courseID",
                table: "UserCourses",
                newName: "IX_UserCourses_courseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups",
                columns: new[] { "userID", "groupID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCourses",
                table: "UserCourses",
                columns: new[] { "userID", "courseID" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_Courses_courseID",
                table: "UserCourses",
                column: "courseID",
                principalTable: "Courses",
                principalColumn: "courseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourses_Users_userID",
                table: "UserCourses",
                column: "userID",
                principalTable: "Users",
                principalColumn: "userID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Groups_groupID",
                table: "UserGroups",
                column: "groupID",
                principalTable: "Groups",
                principalColumn: "groupID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Projects_projectID",
                table: "UserGroups",
                column: "projectID",
                principalTable: "Projects",
                principalColumn: "projectID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_Users_userID",
                table: "UserGroups",
                column: "userID",
                principalTable: "Users",
                principalColumn: "userID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_Courses_courseID",
                table: "UserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCourses_Users_userID",
                table: "UserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Groups_groupID",
                table: "UserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Projects_projectID",
                table: "UserGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_Users_userID",
                table: "UserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserGroups",
                table: "UserGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCourses",
                table: "UserCourses");

            migrationBuilder.RenameTable(
                name: "UserGroups",
                newName: "UserGroup");

            migrationBuilder.RenameTable(
                name: "UserCourses",
                newName: "UserCourse");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroups_projectID",
                table: "UserGroup",
                newName: "IX_UserGroup_projectID");

            migrationBuilder.RenameIndex(
                name: "IX_UserGroups_groupID",
                table: "UserGroup",
                newName: "IX_UserGroup_groupID");

            migrationBuilder.RenameIndex(
                name: "IX_UserCourses_courseID",
                table: "UserCourse",
                newName: "IX_UserCourse_courseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserGroup",
                table: "UserGroup",
                columns: new[] { "userID", "groupID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCourse",
                table: "UserCourse",
                columns: new[] { "userID", "courseID" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourse_Courses_courseID",
                table: "UserCourse",
                column: "courseID",
                principalTable: "Courses",
                principalColumn: "courseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCourse_Users_userID",
                table: "UserCourse",
                column: "userID",
                principalTable: "Users",
                principalColumn: "userID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroup_Groups_groupID",
                table: "UserGroup",
                column: "groupID",
                principalTable: "Groups",
                principalColumn: "groupID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroup_Projects_projectID",
                table: "UserGroup",
                column: "projectID",
                principalTable: "Projects",
                principalColumn: "projectID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroup_Users_userID",
                table: "UserGroup",
                column: "userID",
                principalTable: "Users",
                principalColumn: "userID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
