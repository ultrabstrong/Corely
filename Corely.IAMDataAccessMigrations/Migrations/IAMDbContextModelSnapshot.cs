﻿// <auto-generated />
using System;
using Corely.IAM.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Corely.IAMDataAccessMigrations.Migrations
{
    [DbContext(typeof(IamDbContext))]
    partial class IAMDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
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

            modelBuilder.Entity("Corely.IAM.Accounts.Entities.AccountAsymmetricKeyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("EncryptedPrivateKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("KeyUsedFor")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("ModifiedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("ProviderTypeCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId", "KeyUsedFor")
                        .IsUnique();

                    b.ToTable("AccountAsymmetricKeys", (string)null);
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

            modelBuilder.Entity("Corely.IAM.Accounts.Entities.AccountSymmetricKeyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("EncryptedKey")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("KeyUsedFor")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("ModifiedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("ProviderTypeCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId", "KeyUsedFor")
                        .IsUnique();

                    b.ToTable("AccountSymmetricKeys", (string)null);
                });

            modelBuilder.Entity("Corely.IAM.BasicAuths.Entities.BasicAuthEntity", b =>
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

            modelBuilder.Entity("Corely.IAM.Groups.Entities.GroupEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime>("ModifiedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Groups", (string)null);
                });

            modelBuilder.Entity("Corely.IAM.Roles.Entities.RoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<DateTime>("ModifiedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("Corely.IAM.Users.Entities.UserAsymmetricKeyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("EncryptedPrivateKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("KeyUsedFor")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("ModifiedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("ProviderTypeCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "KeyUsedFor")
                        .IsUnique();

                    b.ToTable("UserAsymmetricKeys", (string)null);
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

                    b.Property<bool>("Disabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(254)
                        .HasColumnType("varchar(254)");

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

            modelBuilder.Entity("Corely.IAM.Users.Entities.UserSymmetricKeyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("EncryptedKey")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("KeyUsedFor")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("ModifiedUtc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TIMESTAMP")
                        .HasDefaultValueSql("(UTC_TIMESTAMP)");

                    b.Property<string>("ProviderTypeCode")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "KeyUsedFor")
                        .IsUnique();

                    b.ToTable("UserSymmetricKeys", (string)null);
                });

            modelBuilder.Entity("GroupEntityRoleEntity", b =>
                {
                    b.Property<int>("GroupsId")
                        .HasColumnType("int");

                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.HasKey("GroupsId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("GroupRoles", (string)null);
                });

            modelBuilder.Entity("GroupEntityUserEntity", b =>
                {
                    b.Property<int>("GroupsId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("GroupsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserGroups", (string)null);
                });

            modelBuilder.Entity("RoleEntityUserEntity", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.Property<int>("UsersId")
                        .HasColumnType("int");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UserRoles", (string)null);
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

            modelBuilder.Entity("Corely.IAM.Accounts.Entities.AccountAsymmetricKeyEntity", b =>
                {
                    b.HasOne("Corely.IAM.Accounts.Entities.AccountEntity", null)
                        .WithMany("AsymmetricKeys")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Corely.IAM.Accounts.Entities.AccountSymmetricKeyEntity", b =>
                {
                    b.HasOne("Corely.IAM.Accounts.Entities.AccountEntity", null)
                        .WithMany("SymmetricKeys")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Corely.IAM.BasicAuths.Entities.BasicAuthEntity", b =>
                {
                    b.HasOne("Corely.IAM.Users.Entities.UserEntity", "User")
                        .WithOne("BasicAuth")
                        .HasForeignKey("Corely.IAM.BasicAuths.Entities.BasicAuthEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Corely.IAM.Groups.Entities.GroupEntity", b =>
                {
                    b.HasOne("Corely.IAM.Accounts.Entities.AccountEntity", "Account")
                        .WithMany("Groups")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Corely.IAM.Roles.Entities.RoleEntity", b =>
                {
                    b.HasOne("Corely.IAM.Accounts.Entities.AccountEntity", "Account")
                        .WithMany("Roles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Corely.IAM.Users.Entities.UserAsymmetricKeyEntity", b =>
                {
                    b.HasOne("Corely.IAM.Users.Entities.UserEntity", null)
                        .WithMany("AsymmetricKeys")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Corely.IAM.Users.Entities.UserSymmetricKeyEntity", b =>
                {
                    b.HasOne("Corely.IAM.Users.Entities.UserEntity", null)
                        .WithMany("SymmetricKeys")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GroupEntityRoleEntity", b =>
                {
                    b.HasOne("Corely.IAM.Groups.Entities.GroupEntity", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Corely.IAM.Roles.Entities.RoleEntity", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GroupEntityUserEntity", b =>
                {
                    b.HasOne("Corely.IAM.Groups.Entities.GroupEntity", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Corely.IAM.Users.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleEntityUserEntity", b =>
                {
                    b.HasOne("Corely.IAM.Roles.Entities.RoleEntity", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Corely.IAM.Users.Entities.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Corely.IAM.Accounts.Entities.AccountEntity", b =>
                {
                    b.Navigation("AsymmetricKeys");

                    b.Navigation("Groups");

                    b.Navigation("Roles");

                    b.Navigation("SymmetricKeys");
                });

            modelBuilder.Entity("Corely.IAM.Users.Entities.UserEntity", b =>
                {
                    b.Navigation("AsymmetricKeys");

                    b.Navigation("BasicAuth");

                    b.Navigation("SymmetricKeys");
                });
#pragma warning restore 612, 618
        }
    }
}
