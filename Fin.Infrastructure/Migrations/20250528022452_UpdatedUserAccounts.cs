using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUserAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_AspNetUsers_FinsightUserId",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "IX_accounts_FinsightUserId",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "FinsightUserId",
                table: "accounts");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_UserId",
                table: "accounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_AspNetUsers_UserId",
                table: "accounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_AspNetUsers_UserId",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "IX_accounts_UserId",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "accounts");

            migrationBuilder.AddColumn<string>(
                name: "FinsightUserId",
                table: "accounts",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_accounts_FinsightUserId",
                table: "accounts",
                column: "FinsightUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_AspNetUsers_FinsightUserId",
                table: "accounts",
                column: "FinsightUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
