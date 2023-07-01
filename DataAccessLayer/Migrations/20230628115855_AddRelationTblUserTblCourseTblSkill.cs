using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class AddRelationTblUserTblCourseTblSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_Course_CourseId",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_User_UserId",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_University_University_UniversityId",
                table: "University");

            migrationBuilder.DropForeignKey(
                name: "FK_User_TrainingPlan_TrainingPlanId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_TrainingPlanId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_University_UniversityId",
                table: "University");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_CourseId",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "TrainingPlanId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "University");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Certificate");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Certificate",
                newName: "SubmitDate");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Certificate",
                newName: "EnrollDate");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Skill",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlatformName",
                table: "Course",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Certificate",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Certificate",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate",
                columns: new[] { "CourseId", "UserId" });

            migrationBuilder.CreateTable(
                name: "CourseSkill",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    RecommendedLevel = table.Column<int>(type: "int", nullable: true),
                    AfterwardLevel = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSkill", x => new { x.SkillId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_CourseSkill_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseSkill_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "UserSkill",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: true),
                    UserId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkill", x => new { x.SkillId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserSkill_Course_UserId",
                        column: x => x.UserId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkill_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkill_User_UserId1",
                        column: x => x.UserId1,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTrainingPlan",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TrainingPlanId = table.Column<int>(type: "int", nullable: false),
                    IsOwner = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTrainingPlan", x => new { x.UserId, x.TrainingPlanId });
                    table.ForeignKey(
                        name: "FK_UserTrainingPlan_TrainingPlan_TrainingPlanId",
                        column: x => x.TrainingPlanId,
                        principalTable: "TrainingPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTrainingPlan_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseSkill_CourseId",
                table: "CourseSkill",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateCriteria_CriteriaId",
                table: "TemplateCriteria",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserId",
                table: "UserSkill",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserId1",
                table: "UserSkill",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserTrainingPlan_TrainingPlanId",
                table: "UserTrainingPlan",
                column: "TrainingPlanId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_Course_CourseId",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_User_UserId",
                table: "Certificate");

            migrationBuilder.DropTable(
                name: "CourseSkill");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "TemplateCriteria");

            migrationBuilder.DropTable(
                name: "UserSkill");

            migrationBuilder.DropTable(
                name: "UserTrainingPlan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "PlatformName",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "SubmitDate",
                table: "Certificate",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "EnrollDate",
                table: "Certificate",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<int>(
                name: "TrainingPlanId",
                table: "User",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UniversityId",
                table: "University",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Certificate",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "Certificate",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Certificate",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Certificate",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Certificate",
                table: "Certificate",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_TrainingPlanId",
                table: "User",
                column: "TrainingPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_University_UniversityId",
                table: "University",
                column: "UniversityId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_CourseId",
                table: "Certificate",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_Course_CourseId",
                table: "Certificate",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_User_UserId",
                table: "Certificate",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_University_University_UniversityId",
                table: "University",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_TrainingPlan_TrainingPlanId",
                table: "User",
                column: "TrainingPlanId",
                principalTable: "TrainingPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
