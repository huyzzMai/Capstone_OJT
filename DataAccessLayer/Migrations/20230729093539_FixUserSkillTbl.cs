using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class FixUserSkillTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_SkillId",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserSkill");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                columns: new[] { "SkillId", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserSkill",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_SkillId",
                table: "UserSkill",
                column: "SkillId");
        }
    }
}
