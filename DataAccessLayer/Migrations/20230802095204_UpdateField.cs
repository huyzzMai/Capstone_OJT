using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class UpdateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataStartColumn",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "DataStartRow",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Skill");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "University",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartCell",
                table: "Template",
                type: "nvarchar(5)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "University");

            migrationBuilder.DropColumn(
                name: "StartCell",
                table: "Template");

            migrationBuilder.AddColumn<int>(
                name: "DataStartColumn",
                table: "Template",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DataStartRow",
                table: "Template",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Skill",
                type: "int",
                nullable: true);
        }
    }
}
