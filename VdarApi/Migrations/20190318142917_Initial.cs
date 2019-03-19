﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VdarApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfirmationKeys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    KeyType = table.Column<string>(nullable: false),
                    HashCode = table.Column<string>(nullable: true),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    ExpireDateUTC = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmationKeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    AccessToken = table.Column<string>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: false),
                    UpdateHashSum = table.Column<string>(nullable: false),
                    FingerPrint = table.Column<string>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    LastRefreshDateUTC = table.Column<DateTime>(nullable: false),
                    UserAgent = table.Column<string>(nullable: true),
                    IP = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SurName = table.Column<string>(nullable: true),
                    FathersName = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true),
                    Password = table.Column<string>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: false),
                    PhoneIsConfirmed = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EmailIsConfirmed = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ActivatedDateUtc = table.Column<DateTime>(nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ActivatedDateUtc", "Birthday", "CreatedDateUtc", "Email", "EmailIsConfirmed", "FathersName", "IsActive", "Login", "Name", "Password", "PhoneIsConfirmed", "PhoneNumber", "Salt", "SurName" },
                values: new object[] { 1, new DateTime(2019, 3, 4, 13, 45, 5, 49, DateTimeKind.Utc), new DateTime(1992, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 3, 18, 14, 29, 17, 50, DateTimeKind.Utc), "admin@google.com", false, "Andreevich", true, "vektor", "Viktor", "lfuE/oWQJUzSUEkr2UGDEYL4F2bTbi0erSBlD3EsDuk=", true, "7771291221", "PYOe1yaW+qQrkz2LzVd7rg==", "Bochikalov" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ActivatedDateUtc", "Birthday", "CreatedDateUtc", "Email", "EmailIsConfirmed", "FathersName", "IsActive", "Login", "Name", "Password", "PhoneIsConfirmed", "PhoneNumber", "Salt", "SurName" },
                values: new object[] { 2, null, null, new DateTime(2019, 3, 18, 14, 29, 17, 51, DateTimeKind.Utc), null, false, null, true, null, "Levon", "5I6AO1tI9shyjUpW98m9IjdNqS9Y2nob/YTfoJv1dXw=", false, "7771940504", "0EJ72/or8TA+K8usgviNsg==", "Kukuyan" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfirmationKeys");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}