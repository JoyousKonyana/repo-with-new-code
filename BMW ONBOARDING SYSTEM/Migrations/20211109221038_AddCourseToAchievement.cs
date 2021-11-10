using Microsoft.EntityFrameworkCore.Migrations;

namespace BMW_ONBOARDING_SYSTEM.Migrations
{
    public partial class AddCourseToAchievement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Achievements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Achievements_CourseId",
                table: "Achievements",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Achievements_Course_CourseId",
                table: "Achievements",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Achievements_Course_CourseId",
                table: "Achievements");

            migrationBuilder.DropIndex(
                name: "IX_Achievements_CourseId",
                table: "Achievements");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Achievements");
        }
    }
}
