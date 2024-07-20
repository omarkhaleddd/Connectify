using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Connectify.Repository.Migrations
{
    public partial class addedPrivacyPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Privacy",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Privacy",
                table: "Post");
        }
    }
}
