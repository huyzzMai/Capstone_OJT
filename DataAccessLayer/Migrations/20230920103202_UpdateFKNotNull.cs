using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class UpdateFKNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_User_UserId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePosition_Course_CourseId",
                table: "CoursePosition");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePosition_Position_PositionId",
                table: "CoursePosition");

            migrationBuilder.DropForeignKey(
                name: "FK_OJTBatch_Template_TemplateId",
                table: "OJTBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_OJTBatch_University_UniversityId",
                table: "OJTBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAccomplished_User_UserId",
                table: "TaskAccomplished");

            migrationBuilder.DropForeignKey(
                name: "FK_Template_University_UniversityId",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPlanDetail_TrainingPlan_TrainingPlanId",
                table: "TrainingPlanDetail");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingPlanId",
                table: "TrainingPlanDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UniversityId",
                table: "Template",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TaskAccomplished",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UniversityId",
                table: "OJTBatch",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "OJTBatch",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "CoursePosition",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CoursePosition",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Attendance",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Config",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Total Working Days Per Month");

            migrationBuilder.UpdateData(
                table: "Config",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Work Hours Required");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_User_UserId",
                table: "Attendance",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePosition_Course_CourseId",
                table: "CoursePosition",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePosition_Position_PositionId",
                table: "CoursePosition",
                column: "PositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OJTBatch_Template_TemplateId",
                table: "OJTBatch",
                column: "TemplateId",
                principalTable: "Template",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OJTBatch_University_UniversityId",
                table: "OJTBatch",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAccomplished_User_UserId",
                table: "TaskAccomplished",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_University_UniversityId",
                table: "Template",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPlanDetail_TrainingPlan_TrainingPlanId",
                table: "TrainingPlanDetail",
                column: "TrainingPlanId",
                principalTable: "TrainingPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_User_UserId",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePosition_Course_CourseId",
                table: "CoursePosition");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursePosition_Position_PositionId",
                table: "CoursePosition");

            migrationBuilder.DropForeignKey(
                name: "FK_OJTBatch_Template_TemplateId",
                table: "OJTBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_OJTBatch_University_UniversityId",
                table: "OJTBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAccomplished_User_UserId",
                table: "TaskAccomplished");

            migrationBuilder.DropForeignKey(
                name: "FK_Template_University_UniversityId",
                table: "Template");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPlanDetail_TrainingPlan_TrainingPlanId",
                table: "TrainingPlanDetail");

            migrationBuilder.AlterColumn<int>(
                name: "TrainingPlanId",
                table: "TrainingPlanDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UniversityId",
                table: "Template",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "TaskAccomplished",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UniversityId",
                table: "OJTBatch",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "OJTBatch",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PositionId",
                table: "CoursePosition",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CoursePosition",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Attendance",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Config",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Số ngày làm quy định trong tháng");

            migrationBuilder.UpdateData(
                table: "Config",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Số giờ làm quy định trong ngày");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_User_UserId",
                table: "Attendance",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePosition_Course_CourseId",
                table: "CoursePosition",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_OJTBatch_University_UniversityId",
                table: "OJTBatch",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAccomplished_User_UserId",
                table: "TaskAccomplished",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Template_University_UniversityId",
                table: "Template",
                column: "UniversityId",
                principalTable: "University",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPlanDetail_TrainingPlan_TrainingPlanId",
                table: "TrainingPlanDetail",
                column: "TrainingPlanId",
                principalTable: "TrainingPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
