using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Corely.DataAccessMigrations.Migrations
{
    /// <inheritdoc />
    public partial class AddedSymmetricKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SymmetricKeyId",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SymmetricKey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SymmetricKey", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_SymmetricKeyId",
                table: "Accounts",
                column: "SymmetricKeyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_SymmetricKey_SymmetricKeyId",
                table: "Accounts",
                column: "SymmetricKeyId",
                principalTable: "SymmetricKey",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_SymmetricKey_SymmetricKeyId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "SymmetricKey");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_SymmetricKeyId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "SymmetricKeyId",
                table: "Accounts");
        }
    }
}
