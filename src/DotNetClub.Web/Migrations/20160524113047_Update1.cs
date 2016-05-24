using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DotNetClub.Web.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCollect",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    TopicID = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCollect", x => new { x.UserID, x.TopicID });
                });

            migrationBuilder.CreateTable(
                name: "UserVote",
                columns: table => new
                {
                    UserID = table.Column<int>(nullable: false),
                    CommentID = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVote", x => new { x.UserID, x.CommentID });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCollect");

            migrationBuilder.DropTable(
                name: "UserVote");
        }
    }
}
