using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class UpdatePositionTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CoursePosition");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CoursePosition");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CoursePosition");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "User",
                newName: "PositionId");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "CoursePosition",
                newName: "PositionId");

            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "OJTBatch",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseBatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    BatchId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseBatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseBatch_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseBatch_OJTBatch_BatchId",
                        column: x => x.BatchId,
                        principalTable: "OJTBatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_PositionId",
                table: "User",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_OJTBatch_TemplateId",
                table: "OJTBatch",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePosition_PositionId",
                table: "CoursePosition",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBatch_BatchId",
                table: "CourseBatch",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBatch_CourseId",
                table: "CourseBatch",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePosition_Position_PositionId",
                table: "CoursePosition",
                column: "PositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OJTBatch_Template_TemplateId",
                table: "OJTBatch",
                column: "TemplateId",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Position_PositionId",
                table: "User",
                column: "PositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePosition_Position_PositionId",
                table: "CoursePosition");

            migrationBuilder.DropForeignKey(
                name: "FK_OJTBatch_Template_TemplateId",
                table: "OJTBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Position_PositionId",
                table: "User");

            migrationBuilder.DropTable(
                name: "CourseBatch");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropIndex(
                name: "IX_User_PositionId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_OJTBatch_TemplateId",
                table: "OJTBatch");

            migrationBuilder.DropIndex(
                name: "IX_CoursePosition_PositionId",
                table: "CoursePosition");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "OJTBatch");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "User",
                newName: "Position");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "CoursePosition",
                newName: "Position");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CoursePosition",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CoursePosition",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CoursePosition",
                type: "datetime2",
                nullable: true);
        }
    }
}
