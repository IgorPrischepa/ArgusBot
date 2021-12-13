﻿// <auto-generated />
using System;
using ArgusBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArgusBot.DAL.Migrations
{
    [DbContext(typeof(MainContext))]
    [Migration("20211209132734_AlteredIndexAsUniqueForCheckInstance")]
    partial class AlteredIndexAsUniqueForCheckInstance
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ArgusBot.DAL.Models.Check", b =>
                {
                    b.Property<int>("CheckId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CorrectAnswer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<int>("QuestionMessageId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SendingTime")
                        .HasColumnType("datetime2");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("CheckId");

                    b.HasIndex("GroupId", "UserId")
                        .IsUnique();

                    b.ToTable("CheckList");
                });

            modelBuilder.Entity("ArgusBot.DAL.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ArgusBot.DAL.Models.GroupAdmin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<long>("TelegramUserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("GroupAdmins");
                });

            modelBuilder.Entity("ArgusBot.DAL.Models.User", b =>
                {
                    b.Property<Guid>("UserGuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Login")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("NormalizedLogin")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NormalizedTelegramLogin")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TelegramId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserGuid");

                    b.HasIndex("NormalizedLogin")
                        .IsUnique()
                        .HasFilter("[NormalizedLogin] IS NOT NULL");

                    b.HasIndex("NormalizedTelegramLogin")
                        .IsUnique()
                        .HasFilter("[NormalizedTelegramLogin] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ArgusBot.DAL.Models.GroupAdmin", b =>
                {
                    b.HasOne("ArgusBot.DAL.Models.Group", "Group")
                        .WithMany("GroupAdmins")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("ArgusBot.DAL.Models.Group", b =>
                {
                    b.Navigation("GroupAdmins");
                });
#pragma warning restore 612, 618
        }
    }
}
