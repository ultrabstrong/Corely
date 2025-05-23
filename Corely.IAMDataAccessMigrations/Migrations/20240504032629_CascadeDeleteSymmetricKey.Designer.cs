﻿// <auto-generated />
using System;
using Corely.IAM.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Corely.IAMDataAccessMigrations.Migrations;

[DbContext(typeof(IamDbContext))]
[Migration("20240504032629_CascadeDeleteSymmetricKey")]
partial class CascadeDeleteSymmetricKey
{
    /// <inheritdoc />
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.4")
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

        modelBuilder.Entity("Corely.IAM.Accounts.Entities.AccountEntity", b =>
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

        modelBuilder.Entity("Corely.IAM.Auth.Entities.BasicAuthEntity", b =>
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

        modelBuilder.Entity("Corely.IAM.Security.Entities.AccountSymmetricKeyEntity", b =>
            {
                b.Property<int>("AccountId")
                    .HasColumnType("int");

                b.Property<DateTime>("CreatedUtc")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TIMESTAMP")
                    .HasDefaultValueSql("(UTC_TIMESTAMP)");

                b.Property<string>("Key")
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnType("varchar(256)");

                b.Property<DateTime>("ModifiedUtc")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TIMESTAMP")
                    .HasDefaultValueSql("(UTC_TIMESTAMP)");

                b.Property<int>("Version")
                    .HasColumnType("int");

                b.HasKey("AccountId");

                b.ToTable("AccountSymmetricKeys", (string)null);
            });

        modelBuilder.Entity("Corely.IAM.Users.Entities.UserDetailsEntity", b =>
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

        modelBuilder.Entity("Corely.IAM.Users.Entities.UserEntity", b =>
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

                b.Property<int>("FailedLoginsSinceLastSuccess")
                    .HasColumnType("int");

                b.Property<DateTime?>("LastFailedLoginUtc")
                    .HasColumnType("datetime(6)");

                b.Property<DateTime?>("LastSuccessfulLoginUtc")
                    .HasColumnType("datetime(6)");

                b.Property<DateTime>("ModifiedUtc")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TIMESTAMP")
                    .HasDefaultValueSql("(UTC_TIMESTAMP)");

                b.Property<int>("TotalFailedLogins")
                    .HasColumnType("int");

                b.Property<int>("TotalSuccessfulLogins")
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
                b.HasOne("Corely.IAM.Accounts.Entities.AccountEntity", null)
                    .WithMany()
                    .HasForeignKey("AccountsId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Corely.IAM.Users.Entities.UserEntity", null)
                    .WithMany()
                    .HasForeignKey("UsersId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Corely.IAM.Auth.Entities.BasicAuthEntity", b =>
            {
                b.HasOne("Corely.IAM.Users.Entities.UserEntity", "User")
                    .WithOne("BasicAuth")
                    .HasForeignKey("Corely.IAM.Auth.Entities.BasicAuthEntity", "UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("User");
            });

        modelBuilder.Entity("Corely.IAM.Security.Entities.AccountSymmetricKeyEntity", b =>
            {
                b.HasOne("Corely.IAM.Accounts.Entities.AccountEntity", null)
                    .WithOne("AccountSymmetricKey")
                    .HasForeignKey("Corely.IAM.Security.Entities.AccountSymmetricKeyEntity", "AccountId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("Corely.IAM.Users.Entities.UserDetailsEntity", b =>
            {
                b.HasOne("Corely.IAM.Users.Entities.UserEntity", "User")
                    .WithOne("Details")
                    .HasForeignKey("Corely.IAM.Users.Entities.UserDetailsEntity", "UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("User");
            });

        modelBuilder.Entity("Corely.IAM.Accounts.Entities.AccountEntity", b =>
            {
                b.Navigation("AccountSymmetricKey")
                    .IsRequired();
            });

        modelBuilder.Entity("Corely.IAM.Users.Entities.UserEntity", b =>
            {
                b.Navigation("BasicAuth");

                b.Navigation("Details");
            });
#pragma warning restore 612, 618
    }
}