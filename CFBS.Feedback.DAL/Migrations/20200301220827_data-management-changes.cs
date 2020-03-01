using Microsoft.EntityFrameworkCore.Migrations;

namespace CFBS.Feedback.DAL.Migrations
{
    public partial class datamanagementchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveQuestions",
                table: "ActiveQuestions");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ImageAnswers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "ActiveQuestions",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveQuestions",
                table: "ActiveQuestions",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveQuestions_LocationID",
                table: "ActiveQuestions",
                column: "LocationID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ActiveQuestions",
                table: "ActiveQuestions");

            migrationBuilder.DropIndex(
                name: "IX_ActiveQuestions_LocationID",
                table: "ActiveQuestions");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ImageAnswers");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "ActiveQuestions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActiveQuestions",
                table: "ActiveQuestions",
                columns: new[] { "LocationID", "QuestionID" });
        }
    }
}
