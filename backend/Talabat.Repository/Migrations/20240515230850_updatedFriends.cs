using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Connectify.Repository.Migrations
{
    public partial class updatedFriends : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FriendName",
                table: "friends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "friends",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendName",
                table: "friends");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "friends");
        }
    }
}
