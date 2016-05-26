using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotNetClub.Web.Migrations
{
    public partial class AddMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommentID = table.Column<int>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    FromUserID = table.Column<int>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    ToUserID = table.Column<int>(nullable: false),
                    TopicID = table.Column<int>(nullable: true),
                    Type = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Message_Comment_CommentID",
                        column: x => x.CommentID,
                        principalTable: "Comment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_User_FromUserID",
                        column: x => x.FromUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Message_User_ToUserID",
                        column: x => x.ToUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_Topic_TopicID",
                        column: x => x.TopicID,
                        principalTable: "Topic",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCollect_TopicID",
                table: "UserCollect",
                column: "TopicID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_CommentID",
                table: "Message",
                column: "CommentID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_FromUserID",
                table: "Message",
                column: "FromUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ToUserID",
                table: "Message",
                column: "ToUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Message_TopicID",
                table: "Message",
                column: "TopicID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCollect_Topic_TopicID",
                table: "UserCollect",
                column: "TopicID",
                principalTable: "Topic",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCollect_Topic_TopicID",
                table: "UserCollect");

            migrationBuilder.DropIndex(
                name: "IX_UserCollect_TopicID",
                table: "UserCollect");

            migrationBuilder.DropTable(
                name: "Message");
        }
    }
}
