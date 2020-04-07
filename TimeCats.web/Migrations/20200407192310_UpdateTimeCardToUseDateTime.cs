using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeCats.Migrations
{
    public partial class UpdateTimeCardToUseDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"TimeCards\" ALTER COLUMN \"timeIn\" TYPE TIMESTAMP USING to_timestamp(\"timeIn\", 'MM/DD/YYYY HH12:MI:SS AM');");
            migrationBuilder.Sql("ALTER TABLE \"TimeCards\" ALTER COLUMN \"timeOut\" TYPE TIMESTAMP USING to_timestamp(\"timeOut\", 'MM/DD/YYYY HH12:MI:SS AM');");
            migrationBuilder.Sql("ALTER TABLE \"TimeCards\" ALTER COLUMN \"createdOn\" TYPE TIMESTAMP USING to_timestamp(\"createdOn\", 'MM/DD/YYYY HH12:MI:SS AM');");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 1,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 108, 149, 64, 2, 205, 192, 21, 182, 167, 205, 98, 34, 47, 219, 138, 140, 47, 107, 79, 88, 94, 62, 24, 196, 67, 32, 178, 230, 118, 19, 43, 223 }, "9S6d/XZcJoPaEf93LEa7l7LiesmzhGpNIH2Z75JnsBM=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 2,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 108, 149, 64, 2, 205, 192, 21, 182, 167, 205, 98, 34, 47, 219, 138, 140, 47, 107, 79, 88, 94, 62, 24, 196, 67, 32, 178, 230, 118, 19, 43, 223 }, "9S6d/XZcJoPaEf93LEa7l7LiesmzhGpNIH2Z75JnsBM=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 3,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 108, 149, 64, 2, 205, 192, 21, 182, 167, 205, 98, 34, 47, 219, 138, 140, 47, 107, 79, 88, 94, 62, 24, 196, 67, 32, 178, 230, 118, 19, 43, 223 }, "9S6d/XZcJoPaEf93LEa7l7LiesmzhGpNIH2Z75JnsBM=" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE \"TimeCards\" ALTER COLUMN \"timeOut\" TYPE text USING to_char(\"timeOut\", 'MM/DD/YYYY HH12:MI:SS AM');");
            migrationBuilder.Sql("ALTER TABLE \"TimeCards\" ALTER COLUMN \"timeIn\" TYPE text USING to_char(\"timeIn\", 'MM/DD/YYYY HH12:MI:SS AM');");
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
