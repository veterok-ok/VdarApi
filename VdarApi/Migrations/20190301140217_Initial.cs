using System;
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
                    Id = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    KeyType = table.Column<string>(nullable: false),
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
                    Id = table.Column<string>(nullable: false),
                    ClientId = table.Column<string>(nullable: false),
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
                    Id = table.Column<string>(nullable: false),
                    Login = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    SurName = table.Column<string>(nullable: false),
                    FathersName = table.Column<string>(nullable: true),
                    Birthday = table.Column<DateTime>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneIsConfirmed = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    EmailIsConfirmed = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ActivatedDateUtc = table.Column<DateTime>(nullable: false),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ActivatedDateUtc", "Birthday", "CreatedDateUtc", "Email", "EmailIsConfirmed", "FathersName", "IsActive", "Login", "Name", "Password", "PhoneIsConfirmed", "PhoneNumber", "SurName" },
                values: new object[] { "3391464a-19ad-4983-b169-5ff06e8eb601", new DateTime(2019, 2, 15, 13, 18, 5, 439, DateTimeKind.Utc), new DateTime(1992, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 3, 1, 14, 2, 17, 440, DateTimeKind.Utc), "admin@google.com", false, "Andreevich", true, "vektor", "Viktor", "123", true, "7771291221", "Bochikalov" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ActivatedDateUtc", "Birthday", "CreatedDateUtc", "Email", "EmailIsConfirmed", "FathersName", "IsActive", "Login", "Name", "Password", "PhoneIsConfirmed", "PhoneNumber", "SurName" },
                values: new object[] { "7fefa895-0f87-446a-852d-5358052df30f", new DateTime(2019, 3, 1, 7, 47, 25, 440, DateTimeKind.Utc), new DateTime(1991, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 3, 1, 14, 2, 17, 440, DateTimeKind.Utc), "l.kukuyan@mail.ru", true, null, true, "lamer", "Levon", "123", false, "7771940504", "Kukuyan" });
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
