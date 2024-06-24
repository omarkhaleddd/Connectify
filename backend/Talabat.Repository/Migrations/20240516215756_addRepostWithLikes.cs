using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Connectify.Repository.Migrations
{
    public partial class addRepostWithLikes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostLikes_Repost_RepostId",
                table: "PostLikes");

            migrationBuilder.DropIndex(
                name: "IX_PostLikes_RepostId",
                table: "PostLikes");

            migrationBuilder.DropColumn(
                name: "RepostId",
                table: "PostLikes");

            migrationBuilder.CreateTable(
                name: "RepostLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepostId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_RepostLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepostLikes_Repost_RepostId",
                        column: x => x.RepostId,
                        principalTable: "Repost",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepostLikes_RepostId",
                table: "RepostLikes",
                column: "RepostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepostLikes");

            migrationBuilder.AddColumn<int>(
                name: "RepostId",
                table: "PostLikes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostLikes_RepostId",
                table: "PostLikes",
                column: "RepostId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Repost_RepostId",
                table: "PostLikes",
                column: "RepostId",
                principalTable: "Repost",
                principalColumn: "Id");
        }
    }
}
