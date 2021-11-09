using Microsoft.EntityFrameworkCore.Migrations;

namespace BMW_ONBOARDING_SYSTEM.Migrations
{
    public partial class ModelNewQuizModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuizQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    QuizId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizQuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Option = table.Column<string>(nullable: true),
                    IsOptionAnswer = table.Column<bool>(nullable: false),
                    QuizQuestionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuizQuestionOptions_QuizQuestions_QuizQuestionId",
                        column: x => x.QuizQuestionId,
                        principalTable: "QuizQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestionOptions_QuizQuestionId",
                table: "QuizQuestionOptions",
                column: "QuizQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizQuestions_QuizId",
                table: "QuizQuestions",
                column: "QuizId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuizQuestionOptions");

            migrationBuilder.DropTable(
                name: "QuizQuestions");
        }
    }
}
