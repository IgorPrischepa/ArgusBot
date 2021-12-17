using Microsoft.EntityFrameworkCore.Migrations;

namespace ArgusBot.DAL.Migrations
{
    public partial class Settings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupSettings_GroupSettingsId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_GroupSettingsId",
                table: "Groups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupSettings",
                table: "GroupSettings");

            migrationBuilder.RenameTable(
                name: "GroupSettings",
                newName: "GroupsSettings");

            migrationBuilder.AddColumn<int>(
                name: "SettingsId",
                table: "Groups",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "GroupsSettings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupsSettings",
                table: "GroupsSettings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_SettingsId",
                table: "Groups",
                column: "SettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupsSettings_SettingsId",
                table: "Groups",
                column: "SettingsId",
                principalTable: "GroupsSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupsSettings_SettingsId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_SettingsId",
                table: "Groups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupsSettings",
                table: "GroupsSettings");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Groups");

            migrationBuilder.RenameTable(
                name: "GroupsSettings",
                newName: "GroupSettings");

            migrationBuilder.AlterColumn<string>(
                name: "GroupId",
                table: "GroupSettings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupSettings",
                table: "GroupSettings",
                column: "Id");

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
    }
}
