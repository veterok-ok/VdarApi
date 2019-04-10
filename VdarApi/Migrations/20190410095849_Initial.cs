using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VdarApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    SurName = table.Column<string>(maxLength: 250, nullable: true),
                    FathersName = table.Column<string>(maxLength: 250, nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    Salt = table.Column<string>(maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: false),
                    PhoneIsConfirmed = table.Column<bool>(nullable: false),
                    Email = table.Column<string>(maxLength: 320, nullable: true),
                    EmailIsConfirmed = table.Column<bool>(nullable: false),
                    EmailIsSubscribe = table.Column<bool>(nullable: false),
                    EmailKeyUnSubscribe = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    ActivatedDateUtc = table.Column<DateTime>(nullable: true),
                    CreatedDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfirmationKeys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    Key = table.Column<string>(maxLength: 10, nullable: false),
                    KeyType = table.Column<string>(maxLength: 25, nullable: false),
                    HashCode = table.Column<string>(nullable: true),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    ExpireDateUTC = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfirmationKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfirmationKeys_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    AccessToken = table.Column<string>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: false),
                    UpdateHashSum = table.Column<string>(nullable: false),
                    FingerPrint = table.Column<string>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false),
                    LastRefreshDateUTC = table.Column<DateTime>(nullable: false),
                    UserAgent = table.Column<string>(nullable: true),
                    IP = table.Column<string>(maxLength: 100, nullable: true),
                    Location = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ActivatedDateUtc", "Birthday", "CreatedDateUtc", "Email", "EmailIsConfirmed", "EmailIsSubscribe", "EmailKeyUnSubscribe", "FathersName", "IsActive", "Login", "Name", "Password", "PhoneIsConfirmed", "PhoneNumber", "Salt", "SurName" },
                values: new object[] { 1, new DateTime(2019, 3, 27, 9, 14, 36, 853, DateTimeKind.Utc), new DateTime(1992, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2019, 4, 10, 9, 58, 48, 886, DateTimeKind.Utc), "admin@google.com", false, false, null, "Andreevich", true, "vektor", "Viktor", "cvgWVx2PzyodoHjQuvNZza7SQzZ/dN3lACijh7STvr0=", true, "7771291221", "RrL2RyNzrMzB6CA7ZRe+9g==", "Bochikalov" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ActivatedDateUtc", "Birthday", "CreatedDateUtc", "Email", "EmailIsConfirmed", "EmailIsSubscribe", "EmailKeyUnSubscribe", "FathersName", "IsActive", "Login", "Name", "Password", "PhoneIsConfirmed", "PhoneNumber", "Salt", "SurName" },
                values: new object[] { 2, null, null, new DateTime(2019, 4, 10, 9, 58, 48, 888, DateTimeKind.Utc), null, false, false, null, null, true, null, "Levon", "prMiHmsITfkH/siMY/aRmpP5epzS0tuKir39cp+dbtw=", false, "7771940504", "fqVPoSLeMzonCyoK0NbBUg==", "Kukuyan" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationKeys_UserId",
                table: "ConfirmationKeys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserId",
                table: "Tokens",
                column: "UserId");
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
