using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeCats.Migrations
{
    public partial class UTConTimeCards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 1,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 221, 146, 85, 13, 248, 184, 150, 167, 115, 254, 166, 67, 39, 121, 95, 249, 45, 14, 105, 190, 117, 30, 25, 126, 95, 82, 97, 77, 16, 192, 235, 91 }, "sjTWQzBCp7jMCGR142G/jdM0v8ocp9NGmexUz6FTSjc=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 2,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 221, 146, 85, 13, 248, 184, 150, 167, 115, 254, 166, 67, 39, 121, 95, 249, 45, 14, 105, 190, 117, 30, 25, 126, 95, 82, 97, 77, 16, 192, 235, 91 }, "sjTWQzBCp7jMCGR142G/jdM0v8ocp9NGmexUz6FTSjc=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 3,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 221, 146, 85, 13, 248, 184, 150, 167, 115, 254, 166, 67, 39, 121, 95, 249, 45, 14, 105, 190, 117, 30, 25, 126, 95, 82, 97, 77, 16, 192, 235, 91 }, "sjTWQzBCp7jMCGR142G/jdM0v8ocp9NGmexUz6FTSjc=" });
        }
    }
}
