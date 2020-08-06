using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TimeCats.Migrations
{
    public partial class NewTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "evalID",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Evals",
                columns: table => new
                {
                    evalID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    evalTemplateID = table.Column<int>(nullable: false),
                    groupID = table.Column<int>(nullable: false),
                    userID = table.Column<int>(nullable: false),
                    number = table.Column<int>(nullable: false),
                    isComplete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evals", x => x.evalID);
                });

            migrationBuilder.CreateTable(
                name: "EvalTemplates",
                columns: table => new
                {
                    evalTemplateID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userID = table.Column<int>(nullable: false),
                    templateName = table.Column<string>(nullable: true),
                    inUse = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvalTemplates", x => x.evalTemplateID);
                });

            migrationBuilder.CreateTable(
                name: "EvalColumn",
                columns: table => new
                {
                    originalID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    evalID = table.Column<int>(nullable: false),
                    firstName = table.Column<string>(nullable: false),
                    lastName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvalColumn", x => x.originalID);
                    table.ForeignKey(
                        name: "FK_EvalColumn_Evals_evalID",
                        column: x => x.evalID,
                        principalTable: "Evals",
                        principalColumn: "evalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvalResponses",
                columns: table => new
                {
                    evalResponseID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    evalID = table.Column<int>(nullable: false),
                    evalTemplateQuestionID = table.Column<int>(nullable: false),
                    userID = table.Column<int>(nullable: false),
                    response = table.Column<string>(nullable: true),
                    firstName = table.Column<string>(nullable: true),
                    lastName = table.Column<string>(nullable: true),
                    evalNumber = table.Column<int>(nullable: false),
                    questionNumber = table.Column<int>(nullable: false),
                    columTotal = table.Column<int>(nullable: false),
                    userAvgerage = table.Column<double>(nullable: false),
                    questionText = table.Column<string>(nullable: true),
                    categoryName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvalResponses", x => x.evalResponseID);
                    table.ForeignKey(
                        name: "FK_EvalResponses_Evals_evalID",
                        column: x => x.evalID,
                        principalTable: "Evals",
                        principalColumn: "evalID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvalTemplateQuestionCategories",
                columns: table => new
                {
                    evalTemplateQuestionCategoryID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    evalTemplateID = table.Column<int>(nullable: false),
                    categoryName = table.Column<string>(nullable: true),
                    number = table.Column<int>(nullable: false),
                    evalID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvalTemplateQuestionCategories", x => x.evalTemplateQuestionCategoryID);
                    table.ForeignKey(
                        name: "FK_EvalTemplateQuestionCategories_Evals_evalID",
                        column: x => x.evalID,
                        principalTable: "Evals",
                        principalColumn: "evalID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalTemplateQuestionCategories_EvalTemplates_evalTemplateID",
                        column: x => x.evalTemplateID,
                        principalTable: "EvalTemplates",
                        principalColumn: "evalTemplateID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvalTemplateQuestions",
                columns: table => new
                {
                    evalTemplateQuestionID = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    evalTemplateID = table.Column<int>(nullable: false),
                    evalTemplateQuestionCategoryID = table.Column<int>(nullable: false),
                    number = table.Column<int>(nullable: false),
                    questionType = table.Column<char>(nullable: false),
                    questionText = table.Column<string>(nullable: true),
                    evalID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvalTemplateQuestions", x => x.evalTemplateQuestionID);
                    table.ForeignKey(
                        name: "FK_EvalTemplateQuestions_Evals_evalID",
                        column: x => x.evalID,
                        principalTable: "Evals",
                        principalColumn: "evalID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EvalTemplateQuestions_EvalTemplates_evalTemplateID",
                        column: x => x.evalTemplateID,
                        principalTable: "EvalTemplates",
                        principalColumn: "evalTemplateID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 1,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 241, 38, 81, 111, 31, 219, 233, 186, 205, 191, 121, 36, 126, 161, 230, 253, 79, 177, 217, 255, 214, 152, 234, 249, 240, 204, 191, 148, 122, 246, 61, 146 }, "JSPeE8IaVUr/ldys5W5lbfOCA4neeb6vUEwnw+X48Y8=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 2,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 241, 38, 81, 111, 31, 219, 233, 186, 205, 191, 121, 36, 126, 161, 230, 253, 79, 177, 217, 255, 214, 152, 234, 249, 240, 204, 191, 148, 122, 246, 61, 146 }, "JSPeE8IaVUr/ldys5W5lbfOCA4neeb6vUEwnw+X48Y8=" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "userID",
                keyValue: 3,
                columns: new[] { "Salt", "password" },
                values: new object[] { new byte[] { 241, 38, 81, 111, 31, 219, 233, 186, 205, 191, 121, 36, 126, 161, 230, 253, 79, 177, 217, 255, 214, 152, 234, 249, 240, 204, 191, 148, 122, 246, 61, 146 }, "JSPeE8IaVUr/ldys5W5lbfOCA4neeb6vUEwnw+X48Y8=" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_evalID",
                table: "Users",
                column: "evalID");

            migrationBuilder.CreateIndex(
                name: "IX_EvalColumn_evalID",
                table: "EvalColumn",
                column: "evalID");

            migrationBuilder.CreateIndex(
                name: "IX_EvalResponses_evalID",
                table: "EvalResponses",
                column: "evalID");

            migrationBuilder.CreateIndex(
                name: "IX_EvalTemplateQuestionCategories_evalID",
                table: "EvalTemplateQuestionCategories",
                column: "evalID");

            migrationBuilder.CreateIndex(
                name: "IX_EvalTemplateQuestionCategories_evalTemplateID",
                table: "EvalTemplateQuestionCategories",
                column: "evalTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_EvalTemplateQuestions_evalID",
                table: "EvalTemplateQuestions",
                column: "evalID");

            migrationBuilder.CreateIndex(
                name: "IX_EvalTemplateQuestions_evalTemplateID",
                table: "EvalTemplateQuestions",
                column: "evalTemplateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Evals_evalID",
                table: "Users",
                column: "evalID",
                principalTable: "Evals",
                principalColumn: "evalID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Evals_evalID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "EvalColumn");

            migrationBuilder.DropTable(
                name: "EvalResponses");

            migrationBuilder.DropTable(
                name: "EvalTemplateQuestionCategories");

            migrationBuilder.DropTable(
                name: "EvalTemplateQuestions");

            migrationBuilder.DropTable(
                name: "Evals");

            migrationBuilder.DropTable(
                name: "EvalTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Users_evalID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "evalID",
                table: "Users");

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
    }
}
