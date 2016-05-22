using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DotNetClub.Web.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    IsBlock = table.Column<bool>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    Salt = table.Column<string>(nullable: false),
                    Signature = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: false),
                    WebSite = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: false),
                    CollectCount = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUserID = table.Column<int>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    LastReplyDate = table.Column<DateTime>(nullable: true),
                    LastReplyUserID = table.Column<int>(nullable: true),
                    Lock = table.Column<bool>(nullable: false),
                    Recommand = table.Column<bool>(nullable: false),
                    ReplyCount = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    Top = table.Column<bool>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    VisitCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Topic_User_CreateUserID",
                        column: x => x.CreateUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Topic_User_LastReplyUserID",
                        column: x => x.LastReplyUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    CreateUserID = table.Column<int>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    ReplyID = table.Column<int>(nullable: true),
                    TopicID = table.Column<int>(nullable: false),
                    Ups = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Comment_User_CreateUserID",
                        column: x => x.CreateUserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comment_Topic_TopicID",
                        column: x => x.TopicID,
                        principalTable: "Topic",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CreateUserID",
                table: "Comment",
                column: "CreateUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_TopicID",
                table: "Comment",
                column: "TopicID");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_CreateUserID",
                table: "Topic",
                column: "CreateUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_LastReplyUserID",
                table: "Topic",
                column: "LastReplyUserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Topic");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
