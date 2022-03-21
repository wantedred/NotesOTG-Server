using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesOTG_Server.Migrations
{
    public partial class addedpublicidtoNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Notes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Notes");
        }
    }
}
