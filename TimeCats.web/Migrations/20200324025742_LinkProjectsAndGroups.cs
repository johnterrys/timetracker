using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeCats.Migrations
{
    public partial class LinkProjectsAndGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Courses_CourseID",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_CourseID",
                table: "Projects");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 1,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 108, 110, 182, 72, 99, 140, 124, 237, 190, 197, 146, 104, 19, 46, 232, 192, 99, 42, 70, 77, 22, 183, 40, 98, 108, 239, 138, 119, 22, 29, 80, 244 }, "490wq6CgXtBoa7zQ0+fZ8oula3VY7n0xBuhEyL43eik=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 2,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 108, 110, 182, 72, 99, 140, 124, 237, 190, 197, 146, 104, 19, 46, 232, 192, 99, 42, 70, 77, 22, 183, 40, 98, 108, 239, 138, 119, 22, 29, 80, 244 }, "490wq6CgXtBoa7zQ0+fZ8oula3VY7n0xBuhEyL43eik=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 3,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 108, 110, 182, 72, 99, 140, 124, 237, 190, 197, 146, 104, 19, 46, 232, 192, 99, 42, 70, 77, 22, 183, 40, 98, 108, 239, 138, 119, 22, 29, 80, 244 }, "490wq6CgXtBoa7zQ0+fZ8oula3VY7n0xBuhEyL43eik=" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 1,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 190, 176, 135, 224, 169, 56, 102, 7, 176, 216, 51, 210, 173, 9, 127, 133, 175, 162, 0, 106, 109, 47, 104, 193, 19, 15, 16, 119, 247, 150, 198, 151 }, "bSr2t3bUhq39QdFZvwPwG1diG4sRMS92KJz0wzcRQqE=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 2,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 190, 176, 135, 224, 169, 56, 102, 7, 176, 216, 51, 210, 173, 9, 127, 133, 175, 162, 0, 106, 109, 47, 104, 193, 19, 15, 16, 119, 247, 150, 198, 151 }, "bSr2t3bUhq39QdFZvwPwG1diG4sRMS92KJz0wzcRQqE=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 3,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 190, 176, 135, 224, 169, 56, 102, 7, 176, 216, 51, 210, 173, 9, 127, 133, 175, 162, 0, 106, 109, 47, 104, 193, 19, 15, 16, 119, 247, 150, 198, 151 }, "bSr2t3bUhq39QdFZvwPwG1diG4sRMS92KJz0wzcRQqE=" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CourseID",
                table: "Projects",
                column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Courses_CourseID",
                table: "Projects",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "courseID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
