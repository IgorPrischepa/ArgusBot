using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArgusBot.DAL.Migrations
{
    public partial class Createchecklisttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckLists",
                columns: table => new
                {
                    CheckId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    QuestionMessageId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    SendingTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckLists", x => x.CheckId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckLists_GroupId_UserId",
                table: "CheckLists",
                columns: new[] { "GroupId", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckLists");
        }
    }
}
