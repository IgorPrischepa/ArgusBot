using Microsoft.EntityFrameworkCore.Migrations;

namespace ArgusBot.DAL.Migrations
{
    public partial class AlteredIndexAsUniqueForCheckInstance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CheckList_GroupId_UserId",
                table: "CheckList");

            migrationBuilder.CreateIndex(
                name: "IX_CheckList_GroupId_UserId",
                table: "CheckList",
                columns: new[] { "GroupId", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CheckList_GroupId_UserId",
                table: "CheckList");

            migrationBuilder.CreateIndex(
                name: "IX_CheckList_GroupId_UserId",
                table: "CheckList",
                columns: new[] { "GroupId", "UserId" });
        }
    }
}
