using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArgusBot.DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    NormalizedLogin = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelegramId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserGuid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedLogin",
                table: "Users",
                column: "NormalizedLogin",
                unique: true,
                filter: "[NormalizedLogin] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
