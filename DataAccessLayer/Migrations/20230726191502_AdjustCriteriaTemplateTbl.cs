using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class AdjustCriteriaTemplateTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCriteria_Criteria_CriteriaId",
                table: "UserCriteria");

            migrationBuilder.DropTable(
                name: "Criteria");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "UserCriteria",
                newName: "UpdatedDate");

            migrationBuilder.RenameColumn(
                name: "CriteriaId",
                table: "UserCriteria",
                newName: "TemplateHeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCriteria_CriteriaId",
                table: "UserCriteria",
                newName: "IX_UserCriteria_TemplateHeaderId");

            migrationBuilder.AlterColumn<double>(
                name: "Point",
                table: "UserCriteria",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserCriteria",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Birthday",
                table: "User",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    DataStartColumn = table.Column<int>(type: "int", nullable: true),
                    DataStartRow = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UniversityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Template_University_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "University",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TemplateHeader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TotalPoint = table.Column<double>(type: "float", nullable: true),
                    MatchedAttribute = table.Column<int>(type: "int", nullable: true),
                    IsCriteria = table.Column<bool>(type: "bit", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TemplateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateHeader_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Template_UniversityId",
                table: "Template",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateHeader_TemplateId",
                table: "TemplateHeader",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCriteria_TemplateHeader_TemplateHeaderId",
                table: "UserCriteria",
                column: "TemplateHeaderId",
                principalTable: "TemplateHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCriteria_TemplateHeader_TemplateHeaderId",
                table: "UserCriteria");

            migrationBuilder.DropTable(
                name: "TemplateHeader");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserCriteria");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "UserCriteria",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "TemplateHeaderId",
                table: "UserCriteria",
                newName: "CriteriaId");

            migrationBuilder.RenameIndex(
                name: "IX_UserCriteria_TemplateHeaderId",
                table: "UserCriteria",
                newName: "IX_UserCriteria_CriteriaId");

            migrationBuilder.AlterColumn<int>(
                name: "Point",
                table: "UserCriteria",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Birthday",
                table: "User",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Criteria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    TotalPoint = table.Column<int>(type: "int", nullable: true),
                    UniversityId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Criteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Criteria_University_UniversityId",
                        column: x => x.UniversityId,
                        principalTable: "University",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_UniversityId",
                table: "Criteria",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCriteria_Criteria_CriteriaId",
                table: "UserCriteria",
                column: "CriteriaId",
                principalTable: "Criteria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
