using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class UpdateRegisterNameTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_Course_CourseId",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_User_UserId",
                table: "Certificate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate");

            migrationBuilder.RenameTable(
                name: "Certificate",
                newName: "Register");

            migrationBuilder.RenameIndex(
                name: "IX_Certificate_UserId",
                table: "Register",
                newName: "IX_Register_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Register",
                table: "Register",
                columns: new[] { "CourseId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Register_Course_CourseId",
                table: "Register",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Register_User_UserId",
                table: "Register",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Register_Course_CourseId",
                table: "Register");

            migrationBuilder.DropForeignKey(
                name: "FK_Register_User_UserId",
                table: "Register");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Register",
                table: "Register");

            migrationBuilder.RenameTable(
                name: "Register",
                newName: "Certificate");

            migrationBuilder.RenameIndex(
                name: "IX_Register_UserId",
                table: "Certificate",
                newName: "IX_Certificate_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate",
                columns: new[] { "CourseId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_Course_CourseId",
                table: "Certificate",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_User_UserId",
                table: "Certificate",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
