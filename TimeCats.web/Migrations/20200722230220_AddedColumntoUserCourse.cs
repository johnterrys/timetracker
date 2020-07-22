using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeCats.Migrations
{
    public partial class AddedColumntoUserCourse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "UserCourses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 1,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 171, 99, 111, 66, 72, 0, 39, 128, 217, 50, 224, 255, 212, 218, 24, 161, 201, 159, 117, 15, 255, 166, 131, 131, 245, 134, 209, 80, 225, 234, 62, 208 }, "eorkD02jm49mg0heIm25i4tVx1P/jiqNc4Db9CCprPo=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 2,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 171, 99, 111, 66, 72, 0, 39, 128, 217, 50, 224, 255, 212, 218, 24, 161, 201, 159, 117, 15, 255, 166, 131, 131, 245, 134, 209, 80, 225, 234, 62, 208 }, "eorkD02jm49mg0heIm25i4tVx1P/jiqNc4Db9CCprPo=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 3,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 171, 99, 111, 66, 72, 0, 39, 128, 217, 50, 224, 255, 212, 218, 24, 161, 201, 159, 117, 15, 255, 166, 131, 131, 245, 134, 209, 80, 225, 234, 62, 208 }, "eorkD02jm49mg0heIm25i4tVx1P/jiqNc4Db9CCprPo=" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "UserCourses");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 1,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 113, 145, 55, 59, 70, 197, 185, 31, 130, 117, 232, 69, 175, 217, 168, 171, 175, 58, 236, 189, 13, 229, 171, 149, 218, 219, 173, 213, 209, 107, 203, 218 }, "LCDlsInuJ03Pun39kPCixsnG/1QWtMmBhkNb7BG3Lmk=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 2,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 113, 145, 55, 59, 70, 197, 185, 31, 130, 117, 232, 69, 175, 217, 168, 171, 175, 58, 236, 189, 13, 229, 171, 149, 218, 219, 173, 213, 209, 107, 203, 218 }, "LCDlsInuJ03Pun39kPCixsnG/1QWtMmBhkNb7BG3Lmk=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 3,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 113, 145, 55, 59, 70, 197, 185, 31, 130, 117, 232, 69, 175, 217, 168, 171, 175, 58, 236, 189, 13, 229, 171, 149, 218, 219, 173, 213, 209, 107, 203, 218 }, "LCDlsInuJ03Pun39kPCixsnG/1QWtMmBhkNb7BG3Lmk=" });
        }
    }
}
