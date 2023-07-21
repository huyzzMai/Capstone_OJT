using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class FixUserSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInit",
                table: "UserSkill");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "UserSkill",
                newName: "InitLevel");

            migrationBuilder.AddColumn<int>(
                name: "CurrentLevel",
                table: "UserSkill",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentLevel",
                table: "UserSkill");

            migrationBuilder.RenameColumn(
                name: "InitLevel",
                table: "UserSkill",
                newName: "Level");

            migrationBuilder.AddColumn<bool>(
                name: "IsInit",
                table: "UserSkill",
                type: "bit",
                nullable: true);
        }
    }
}
