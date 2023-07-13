using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class UpdateDbVersion12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Course_UserId",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_User_UserId1",
                table: "UserSkill");

            migrationBuilder.DropTable(
                name: "TemplateCriteria");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserId1",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TrainingPlan");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Criteria");

            migrationBuilder.DropColumn(
                name: "IsCompulsory",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "AvatarUrl",
                table: "User",
                newName: "AvatarURL");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserSkill",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsInit",
                table: "UserSkill",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainerIdMark",
                table: "UserCriteria",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImgURL",
                table: "University",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEvaluativeTask",
                table: "TrainingPlanDetail",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Criteria",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UniversityId",
                table: "Criteria",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompulsory",
                table: "CoursePosition",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Course",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_SkillId",
                table: "UserSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Criteria_UniversityId",
                table: "Criteria",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Criteria_University_UniversityId",
                table: "Criteria",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_User_UserId",
                table: "UserSkill",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Criteria_University_UniversityId",
                table: "Criteria");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_User_UserId",
                table: "UserSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_SkillId",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_Criteria_UniversityId",
                table: "Criteria");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "IsInit",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "TrainerIdMark",
                table: "UserCriteria");

            migrationBuilder.DropColumn(
                name: "ImgURL",
                table: "University");

            migrationBuilder.DropColumn(
                name: "IsEvaluativeTask",
                table: "TrainingPlanDetail");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Criteria");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "Criteria");

            migrationBuilder.DropColumn(
                name: "IsCompulsory",
                table: "CoursePosition");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "AvatarURL",
                table: "User",
                newName: "AvatarUrl");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "UserSkill",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "User",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TrainingPlan",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Skill",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Criteria",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompulsory",
                table: "Course",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Course",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                columns: new[] { "SkillId", "UserId" });

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    UniversityId = table.Column<int>(type: "int", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                name: "TemplateCriteria",
                columns: table => new
                {
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    CriteriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateCriteria", x => new { x.TemplateId, x.CriteriaId });
                    table.ForeignKey(
                        name: "FK_TemplateCriteria_Criteria_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "Criteria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateCriteria_Template_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Template",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserId1",
                table: "UserSkill",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Template_UniversityId",
                table: "Template",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateCriteria_CriteriaId",
                table: "TemplateCriteria",
                column: "CriteriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Course_UserId",
                table: "UserSkill",
                column: "UserId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_User_UserId1",
                table: "UserSkill",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
