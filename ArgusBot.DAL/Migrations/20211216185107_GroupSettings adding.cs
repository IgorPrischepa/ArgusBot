using Microsoft.EntityFrameworkCore.Migrations;

namespace ArgusBot.DAL.Migrations
{
    public partial class GroupSettingsadding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupSettingsId",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GroupSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCpatchaEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSettings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupSettingsId",
                table: "Groups",
                column: "GroupSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupSettings_GroupSettingsId",
                table: "Groups",
                column: "GroupSettingsId",
                principalTable: "GroupSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupSettings_GroupSettingsId",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "GroupSettings");

            migrationBuilder.DropIndex(
                name: "IX_Groups_GroupSettingsId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupSettingsId",
                table: "Groups");
        }
    }
}
