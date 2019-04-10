using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VdarApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Info");

            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "Info",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "Info",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Info",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Login = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 250, nullable: true),
                    SurName = table.Column<string>(maxLength: 250, nullable: true),
                    FathersName = table.Column<string>(maxLength: 250, nullable: true),
                    Birthday = table.Column<DateTime>(nullable: true),
                    CityId = table.Column<int>(nullable: true),
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
                    table.ForeignKey(
                        name: "FK_Users_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "Info",
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConfirmationKeys",
                schema: "Identity",
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
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                schema: "Identity",
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
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Info",
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Казахстан" });

            migrationBuilder.InsertData(
                schema: "Info",
                table: "Cities",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Алматы" },
                    { 2, 1, "Нур-Султан" },
                    { 3, 1, "Караганда" },
                    { 4, 1, "Кызылорда" },
                    { 5, 1, "Тараз" },
                    { 6, 1, "Семипалатинск" },
                    { 7, 1, "Павлодар" }
                });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Users",
                columns: new[] { "Id", "ActivatedDateUtc", "Birthday", "CityId", "CreatedDateUtc", "Email", "EmailIsConfirmed", "EmailIsSubscribe", "EmailKeyUnSubscribe", "FathersName", "IsActive", "Login", "Name", "Password", "PhoneIsConfirmed", "PhoneNumber", "Salt", "SurName" },
                values: new object[] { 1, new DateTime(2019, 3, 27, 10, 33, 19, 541, DateTimeKind.Utc), new DateTime(1992, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2019, 4, 10, 11, 17, 31, 542, DateTimeKind.Utc), "admin@google.com", false, false, null, "Andreevich", true, "vektor", "Viktor", "k/MvNZCApSdG+YWc8i52Ce58qLf1ckL+paw6rOUCCBk=", true, "7771291221", "WHZjTtCZ45yg+/OrCOatWg==", "Bochikalov" });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Users",
                columns: new[] { "Id", "ActivatedDateUtc", "Birthday", "CityId", "CreatedDateUtc", "Email", "EmailIsConfirmed", "EmailIsSubscribe", "EmailKeyUnSubscribe", "FathersName", "IsActive", "Login", "Name", "Password", "PhoneIsConfirmed", "PhoneNumber", "Salt", "SurName" },
                values: new object[] { 2, null, null, 1, new DateTime(2019, 4, 10, 11, 17, 31, 545, DateTimeKind.Utc), null, false, false, null, null, true, null, "Levon", "mAW8U7xuKGjfX71QwvQoNR0XRJonbZCc6XjQKZy1C7w=", false, "7771940504", "/Oo2pa+86NtK15v5TUKCIw==", "Kukuyan" });

            migrationBuilder.CreateIndex(
                name: "IX_ConfirmationKeys_UserId",
                schema: "Identity",
                table: "ConfirmationKeys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserId",
                schema: "Identity",
                table: "Tokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CityId",
                schema: "Identity",
                table: "Users",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                schema: "Info",
                table: "Cities",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfirmationKeys",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Tokens",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "Info");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "Info");
        }
    }
}
