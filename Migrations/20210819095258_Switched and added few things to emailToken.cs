using Microsoft.EntityFrameworkCore.Migrations;

namespace NotesOTG_Server.Migrations
{
    public partial class SwitchedandaddedfewthingstoemailToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "EmailVerificationTokens");

            migrationBuilder.AddColumn<string>(
                name: "InternalToken",
                table: "EmailVerificationTokens",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicToken",
                table: "EmailVerificationTokens",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InternalToken",
                table: "EmailVerificationTokens");

            migrationBuilder.DropColumn(
                name: "PublicToken",
                table: "EmailVerificationTokens");

            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "EmailVerificationTokens",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
