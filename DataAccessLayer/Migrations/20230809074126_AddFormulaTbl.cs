using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class AddFormulaTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseBatch");

            migrationBuilder.AddColumn<int>(
                name: "FormulaId",
                table: "TemplateHeader",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Formula",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Calculation = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formula", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateHeader_FormulaId",
                table: "TemplateHeader",
                column: "FormulaId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateHeader_Formula_FormulaId",
                table: "TemplateHeader",
                column: "FormulaId",
                principalTable: "Formula",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateHeader_Formula_FormulaId",
                table: "TemplateHeader");

            migrationBuilder.DropTable(
                name: "Formula");

            migrationBuilder.DropIndex(
                name: "IX_TemplateHeader_FormulaId",
                table: "TemplateHeader");

            migrationBuilder.DropColumn(
                name: "FormulaId",
                table: "TemplateHeader");

            migrationBuilder.CreateTable(
                name: "CourseBatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchId = table.Column<int>(type: "int", nullable: true),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_CourseBatch_BatchId",
                table: "CourseBatch",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBatch_CourseId",
                table: "CourseBatch",
                column: "CourseId");
        }
    }
}
