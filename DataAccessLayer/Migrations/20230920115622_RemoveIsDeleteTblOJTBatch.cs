using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class RemoveIsDeleteTblOJTBatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_University_UniversityId",
                table: "Template");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "OJTBatch");

            migrationBuilder.AddForeignKey(
                name: "FK_Template_University_UniversityId",
                table: "Template",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Template_University_UniversityId",
                table: "Template");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "OJTBatch",
                type: "bit",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_University_UniversityId",
                table: "Template",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "Id");
        }
    }
}
