using Microsoft.EntityFrameworkCore.Migrations;

namespace ArgusBot.DAL.Migrations
{
    public partial class AddNormalizedTelegramAccountfiled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedTelegramLogin",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_NormalizedTelegramLogin",
                table: "Users",
                column: "NormalizedTelegramLogin",
                unique: true,
                filter: "[NormalizedTelegramLogin] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_NormalizedTelegramLogin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedTelegramLogin",
                table: "Users");
        }
    }
}
