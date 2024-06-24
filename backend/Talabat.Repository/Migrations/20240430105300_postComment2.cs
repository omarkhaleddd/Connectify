using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Connectify.Repository.Migrations
{
    public partial class postComment2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Post_postId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "postId",
                table: "Comment",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_postId",
                table: "Comment",
                newName: "IX_Comment_PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Comment",
                newName: "postId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                newName: "IX_Comment_postId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Post_postId",
                table: "Comment",
                column: "postId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
