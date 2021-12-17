using Microsoft.EntityFrameworkCore.Migrations;

namespace ArgusBot.DAL.Migrations
{
    public partial class ChangefieldinSttings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupsSettings_SettingsId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_SettingsId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupSettingsId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                table: "Groups");

            migrationBuilder.AddColumn<long>(
                name: "TelegramChatId",
                table: "GroupsSettings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_GroupsSettings_GroupId",
                table: "GroupsSettings",
                column: "GroupId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupsSettings_Groups_GroupId",
                table: "GroupsSettings",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupsSettings_Groups_GroupId",
                table: "GroupsSettings");

            migrationBuilder.DropIndex(
                name: "IX_GroupsSettings_GroupId",
                table: "GroupsSettings");

            migrationBuilder.DropColumn(
                name: "TelegramChatId",
                table: "GroupsSettings");

            migrationBuilder.AddColumn<int>(
                name: "GroupSettingsId",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SettingsId",
                table: "Groups",
                type: "int",
                nullable: true);

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
    }
}
