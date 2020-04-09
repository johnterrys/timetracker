using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeCats.Migrations
{
    public partial class UpdateTimeCardToUseDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"TimeCards\" ALTER COLUMN \"timeIn\" TYPE TIMESTAMP USING to_timestamp(\"timeIn\", 'MM/DD/YYYY HH12:MI AM');");
            migrationBuilder.Sql("ALTER TABLE \"TimeCards\" ALTER COLUMN \"timeOut\" TYPE TIMESTAMP USING to_timestamp(\"timeOut\", 'MM/DD/YYYY HH12:MI AM');");
            migrationBuilder.Sql("ALTER TABLE \"TimeCards\" ALTER COLUMN \"createdOn\" TYPE TIMESTAMP USING to_timestamp(\"createdOn\", 'MM/DD/YYYY HH12:MI:SS AM');");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"TimeCards\" ALTER COLUMN \"timeIn\" TYPE text USING to_char(\"timeIn\", 'MM/DD/YYYY HH12:MI AM');");
            migrationBuilder.Sql("ALTER TABLE \"TimeCards\" ALTER COLUMN \"timeOut\" TYPE text USING to_char(\"timeOut\", 'MM/DD/YYYY HH12:MI AM');");
            migrationBuilder.Sql("ALTER TABLE \"TimeCards\" ALTER COLUMN \"createdOn\" TYPE text USING to_char(\"createdOn\", 'MM/DD/YYYY HH12:MI:SS AM');");
            
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
    }
}
