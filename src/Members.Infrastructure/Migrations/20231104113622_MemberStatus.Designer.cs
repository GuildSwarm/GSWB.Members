﻿// <auto-generated />
using System;
using Members.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Members.Infrastructure.Migrations
{
    [DbContext(typeof(MembersDbContext))]
    [Migration("20231104113622_MemberStatus")]
    partial class MemberStatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MemberRole", b =>
                {
                    b.Property<Guid>("MembersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RolesId")
                        .HasColumnType("uuid");

                    b.HasKey("MembersId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("MemberRole");
                });

            modelBuilder.Entity("Members.Domain.Entities.IncidentReport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccusedId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("AccuserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)");

                    b.Property<Guid?>("SentenceId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.HasKey("Id");

                    b.HasIndex("AccusedId");

                    b.HasIndex("AccuserId");

                    b.HasIndex("SentenceId");

                    b.ToTable("IncidentReports");
                });

            modelBuilder.Entity("Members.Domain.Entities.Member", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DiscordAvatarUrl")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("DiscordGuildDisplayName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<decimal>("DiscordUserId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("DiscordUserName")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<string>("GameHandle")
                        .HasColumnType("text");

                    b.Property<string>("GameHandleVerificationCode")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)");

                    b.Property<bool>("IsGameHandleVerified")
                        .HasColumnType("boolean");

                    b.Property<string>("SpectrumCommunityMoniker")
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("VerificationCodeExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DiscordUserId")
                        .IsUnique();

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Members.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<decimal>("DiscordRoleId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Permissions")
                        .HasColumnType("integer");

                    b.Property<byte>("Position")
                        .HasColumnType("smallint");

                    b.Property<byte>("RoleType")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.HasIndex("DiscordRoleId")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Members.Domain.Entities.Sentence", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("JudgeId")
                        .HasColumnType("uuid");

                    b.Property<int>("SentenceType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("JudgeId");

                    b.ToTable("Sentences");
                });

            modelBuilder.Entity("Members.Domain.Entities.VerifyCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)");

                    b.Property<DateTimeOffset>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("VerifyCodes");
                });

            modelBuilder.Entity("MemberRole", b =>
                {
                    b.HasOne("Members.Domain.Entities.Member", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Members.Domain.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Members.Domain.Entities.IncidentReport", b =>
                {
                    b.HasOne("Members.Domain.Entities.Member", "Accused")
                        .WithMany()
                        .HasForeignKey("AccusedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Members.Domain.Entities.Member", "Accuser")
                        .WithMany()
                        .HasForeignKey("AccuserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Members.Domain.Entities.Sentence", "Sentence")
                        .WithMany()
                        .HasForeignKey("SentenceId");

                    b.Navigation("Accused");

                    b.Navigation("Accuser");

                    b.Navigation("Sentence");
                });

            modelBuilder.Entity("Members.Domain.Entities.Sentence", b =>
                {
                    b.HasOne("Members.Domain.Entities.Member", "Judge")
                        .WithMany()
                        .HasForeignKey("JudgeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Judge");
                });

            modelBuilder.Entity("Members.Domain.Entities.VerifyCode", b =>
                {
                    b.HasOne("Members.Domain.Entities.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");
                });
#pragma warning restore 612, 618
        }
    }
}
