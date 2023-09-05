using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class EditUserUniversityTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "University");

            migrationBuilder.DropColumn(
                name: "IsEvaluativeTask",
                table: "TrainingPlanDetail");

            migrationBuilder.AddColumn<string>(
                name: "UniversityCode",
                table: "University",
                type: "nvarchar(MAX)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniversityCode",
                table: "University");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "University",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEvaluativeTask",
                table: "TrainingPlanDetail",
                type: "bit",
                nullable: true);
        }
    }
}
