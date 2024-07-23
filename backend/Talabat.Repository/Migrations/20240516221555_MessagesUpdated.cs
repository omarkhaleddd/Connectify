using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Connectify.Repository.Migrations
{
    public partial class MessagesUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "userId",
                table: "messages",
                newName: "senderName");

            migrationBuilder.RenameColumn(
                name: "displayName",
                table: "messages",
                newName: "senderId");

            migrationBuilder.AddColumn<string>(
                name: "recieverId",
                table: "messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "recieverName",
                table: "messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "recieverId",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "recieverName",
                table: "messages");

            migrationBuilder.RenameColumn(
                name: "senderName",
                table: "messages",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "senderId",
                table: "messages",
                newName: "displayName");
        }
    }
}
