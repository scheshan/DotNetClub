using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DotNetClub.Core.Data;

namespace DotNetClub.Web.Migrations
{
    [DbContext(typeof(ClubContext))]
    [Migration("20160520141233_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20896")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DotNetClub.Core.Entity.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreateDate");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 20);

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
        }
    }
}
