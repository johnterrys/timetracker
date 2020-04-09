using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeCats.Migrations
{
    public partial class UpdateTimeCardTimeOutToBeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "timeOut",
                table: "TimeCards",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "timeIn",
                table: "TimeCards",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "createdOn",
                table: "TimeCards",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "timeOut",
                table: "TimeCards",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "timeIn",
                table: "TimeCards",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "createdOn",
                table: "TimeCards",
                type: "text",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 1,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 106, 171, 87, 41, 99, 19, 102, 208, 212, 61, 201, 0, 71, 229, 253, 207, 249, 70, 58, 212, 65, 3, 135, 0, 58, 14, 96, 192, 218, 86, 128, 82 }, "t3nJz5u8ykHpMK/tiinD+P1OhurXJf92ssXxDTE7vRc=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 2,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 106, 171, 87, 41, 99, 19, 102, 208, 212, 61, 201, 0, 71, 229, 253, 207, 249, 70, 58, 212, 65, 3, 135, 0, 58, 14, 96, 192, 218, 86, 128, 82 }, "t3nJz5u8ykHpMK/tiinD+P1OhurXJf92ssXxDTE7vRc=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 3,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 106, 171, 87, 41, 99, 19, 102, 208, 212, 61, 201, 0, 71, 229, 253, 207, 249, 70, 58, 212, 65, 3, 135, 0, 58, 14, 96, 192, 218, 86, 128, 82 }, "t3nJz5u8ykHpMK/tiinD+P1OhurXJf92ssXxDTE7vRc=" });
        }
    }
}
