using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Connectify.Repository.Migrations
{
    public partial class addRepost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepostId",
                table: "PostLikes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RepostId",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Repost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuthorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdateBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeleteBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repost_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_RepostId",
                table: "PostLikes",
                column: "RepostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_RepostId",
                table: "Comment",
                column: "RepostId");

            migrationBuilder.CreateIndex(
                name: "IX_Repost_PostId",
                table: "Repost",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Repost_RepostId",
                table: "Comment",
                column: "RepostId",
                principalTable: "Repost",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Repost_RepostId",
                table: "PostLikes",
                column: "RepostId",
                principalTable: "Repost",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Repost_RepostId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_PostLikes_Repost_RepostId",
                table: "PostLikes");

            migrationBuilder.DropTable(
                name: "Repost");

            migrationBuilder.DropIndex(
                name: "IX_PostLikes_RepostId",
                table: "PostLikes");

            migrationBuilder.DropIndex(
                name: "IX_Comment_RepostId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "RepostId",
                table: "PostLikes");

            migrationBuilder.DropColumn(
                name: "RepostId",
                table: "Comment");
        }
    }
}
