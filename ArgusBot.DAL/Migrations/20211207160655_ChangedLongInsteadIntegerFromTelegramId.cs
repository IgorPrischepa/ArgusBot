using Microsoft.EntityFrameworkCore.Migrations;

namespace ArgusBot.DAL.Migrations
{
    public partial class ChangedLongInsteadIntegerFromTelegramId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupAdmin_Groups_GroupId",
                table: "GroupAdmin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupAdmin",
                table: "GroupAdmin");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GroupAdmin");

            migrationBuilder.RenameTable(
                name: "GroupAdmin",
                newName: "GroupAdmins");

            migrationBuilder.RenameIndex(
                name: "IX_GroupAdmin_GroupId",
                table: "GroupAdmins",
                newName: "IX_GroupAdmins_GroupId");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "CheckList",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "TelegramUserId",
                table: "GroupAdmins",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupAdmins",
                table: "GroupAdmins",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupAdmins_Groups_GroupId",
                table: "GroupAdmins",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupAdmins_Groups_GroupId",
                table: "GroupAdmins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupAdmins",
                table: "GroupAdmins");

            migrationBuilder.DropColumn(
                name: "TelegramUserId",
                table: "GroupAdmins");

            migrationBuilder.RenameTable(
                name: "GroupAdmins",
                newName: "GroupAdmin");

            migrationBuilder.RenameIndex(
                name: "IX_GroupAdmins_GroupId",
                table: "GroupAdmin",
                newName: "IX_GroupAdmin_GroupId");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "CheckList",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "GroupAdmin",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupAdmin",
                table: "GroupAdmin",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupAdmin_Groups_GroupId",
                table: "GroupAdmin",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
