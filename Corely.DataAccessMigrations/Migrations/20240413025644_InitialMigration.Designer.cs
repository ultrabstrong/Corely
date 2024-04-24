﻿// <auto-generated />
using System;
using Corely.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Corely.DataAccessMigrations.Migrations
{
    [DbContext(typeof(AccountManagementDbContext))]
    [Migration("20240413025644_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("AccountEntityUserEntity", b =>
                {
                    b.Property<int>("AccountsId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("AccountsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("AccountEntityUserEntity");
                });

            modelBuilder.Entity("Corely.Domain.Entities.Accounts.AccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<DateTime>("ModifiedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.HasKey("Id");

                    b.HasIndex("AccountName")
                        .IsUnique();

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("Corely.Domain.Entities.Auth.BasicAuthEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<DateTime>("ModifiedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("BasicAuths", (string)null);
                });

            modelBuilder.Entity("Corely.Domain.Entities.Users.UserDetailsEntity", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<DateTime>("ModifiedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("Name")
                        .HasMaxLength(4000)
                        .HasColumnType("varchar(4000)");

                    b.Property<string>("Phone")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("longblob");

                    b.HasKey("UserId");

                    b.ToTable("UserDetails", (string)null);
                });

            modelBuilder.Entity("Corely.Domain.Entities.Users.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("varchar(254)");

                    b.Property<bool>("Enabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(true);

                    b.Property<int>("FailedLogins")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastFailedLoginUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("LastLoginUtc")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("ModifiedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<int>("SuccessfulLogins")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("AccountEntityUserEntity", b =>
                {
                    b.HasOne("Corely.Domain.Entities.Accounts.AccountEntity", null)
                        .WithMany()
                        .HasForeignKey("AccountsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Corely.Domain.Entities.Users.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Corely.Domain.Entities.Auth.BasicAuthEntity", b =>
                {
                    b.HasOne("Corely.Domain.Entities.Users.UserEntity", "User")
                        .WithOne("BasicAuth")
                        .HasForeignKey("Corely.Domain.Entities.Auth.BasicAuthEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Corely.Domain.Entities.Users.UserDetailsEntity", b =>
                {
                    b.HasOne("Corely.Domain.Entities.Users.UserEntity", "User")
                        .WithOne("Details")
                        .HasForeignKey("Corely.Domain.Entities.Users.UserDetailsEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Corely.Domain.Entities.Users.UserEntity", b =>
                {
                    b.Navigation("BasicAuth");

                    b.Navigation("Details");
                });
#pragma warning restore 612, 618
        }
    }
}
