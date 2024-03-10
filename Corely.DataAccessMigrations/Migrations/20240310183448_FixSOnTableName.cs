using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corely.DataAccessMigrations.Migrations
{
    /// <inheritdoc />
    public partial class FixSOnTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetailss_Users_UserId",
                table: "UserDetailss");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDetailss",
                table: "UserDetailss");

            migrationBuilder.RenameTable(
                name: "UserDetailss",
                newName: "UserDetails");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetails_Users_UserId",
                table: "UserDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDetails_Users_UserId",
                table: "UserDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails");

            migrationBuilder.RenameTable(
                name: "UserDetails",
                newName: "UserDetailss");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDetailss",
                table: "UserDetailss",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDetailss_Users_UserId",
                table: "UserDetailss",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
