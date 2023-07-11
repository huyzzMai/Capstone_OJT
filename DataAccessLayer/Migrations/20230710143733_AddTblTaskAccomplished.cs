using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class AddTblTaskAccomplished : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrelloId",
                table: "User",
                type: "nvarchar(40)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskAccomplished",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(40)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    DueDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AccomplishDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskAccomplished", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskAccomplished_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAccomplished_UserId",
                table: "TaskAccomplished",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskAccomplished");

            migrationBuilder.DropColumn(
                name: "TrelloId",
                table: "User");
        }
    }
}
