using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DotNetClub.Core.Data;

namespace DotNetClub.Web.Migrations
{
    [DbContext(typeof(ClubContext))]
    [Migration("20160524113047_Update1")]
    partial class Update1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20896")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DotNetClub.Core.Entity.Comment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("CreateUserID");

                    b.Property<bool>("IsDelete");

                    b.Property<int?>("ReplyID");

                    b.Property<int>("TopicID");

                    b.Property<int>("Ups");

                    b.HasKey("ID");

                    b.HasIndex("CreateUserID");

                    b.HasIndex("TopicID");

                    b.ToTable("Comments");

                    b.HasAnnotation("SqlServer:TableName", "Comment");
                });

            modelBuilder.Entity("DotNetClub.Core.Entity.Topic", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<int>("CollectCount");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("CreateDate");

                    b.Property<int>("CreateUserID");

                    b.Property<bool>("IsDelete");

                    b.Property<DateTime?>("LastReplyDate");

                    b.Property<int?>("LastReplyUserID");

                    b.Property<bool>("Lock");

                    b.Property<bool>("Recommand");

                    b.Property<int>("ReplyCount");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("Top");

                    b.Property<DateTime>("UpdateDate");

                    b.Property<int>("VisitCount");

                    b.HasKey("ID");

                    b.HasIndex("CreateUserID");

                    b.HasIndex("LastReplyUserID");

                    b.ToTable("Topics");

                    b.HasAnnotation("SqlServer:TableName", "Topic");
                });

            modelBuilder.Entity("DotNetClub.Core.Entity.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<bool>("IsBlock");

                    b.Property<string>("Location")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 100);

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Signature")
                        .HasAnnotation("MaxLength", 500);

                    b.Property<string>("Token")
                        .HasAnnotation("MaxLength", 32);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

                    b.Property<string>("WebSite")
                        .HasAnnotation("MaxLength", 200);

                    b.HasKey("ID");

                    b.ToTable("Users");

                    b.HasAnnotation("SqlServer:TableName", "User");
                });

            modelBuilder.Entity("DotNetClub.Core.Entity.UserCollect", b =>
                {
                    b.Property<int>("UserID");

                    b.Property<int>("TopicID");

                    b.Property<DateTime>("CreateDate");

                    b.HasKey("UserID", "TopicID");

                    b.ToTable("UserCollects");

                    b.HasAnnotation("SqlServer:TableName", "UserCollect");
                });

            modelBuilder.Entity("DotNetClub.Core.Entity.UserVote", b =>
                {
                    b.Property<int>("UserID");

                    b.Property<int>("CommentID");

                    b.Property<DateTime>("CreateDate");

                    b.HasKey("UserID", "CommentID");

                    b.ToTable("UserVotes");

                    b.HasAnnotation("SqlServer:TableName", "UserVote");
                });

            modelBuilder.Entity("DotNetClub.Core.Entity.Comment", b =>
                {
                    b.HasOne("DotNetClub.Core.Entity.User")
                        .WithMany()
                        .HasForeignKey("CreateUserID");

                    b.HasOne("DotNetClub.Core.Entity.Topic")
                        .WithMany()
                        .HasForeignKey("TopicID");
                });

            modelBuilder.Entity("DotNetClub.Core.Entity.Topic", b =>
                {
                    b.HasOne("DotNetClub.Core.Entity.User")
                        .WithMany()
                        .HasForeignKey("CreateUserID");

                    b.HasOne("DotNetClub.Core.Entity.User")
                        .WithMany()
                        .HasForeignKey("LastReplyUserID");
                });
        }
    }
}
