using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VdarApi.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Action");

            migrationBuilder.EnsureSchema(
                name: "Info");

            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "Owners",
                schema: "Action",
                columns: table => new
                {
                    OwnerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CompanyName = table.Column<string>(nullable: true),
                    ContactMobile = table.Column<string>(nullable: true),
                    ContactEmail = table.Column<string>(nullable: true),
                    AboutCompanyHTML = table.Column<string>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Sault = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    RegistrationDate = table.Column<DateTime>(nullable: false),
                    LastActivity = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.OwnerId);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                schema: "Action",
                columns: table => new
                {
                    StockId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 500, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    StartDateUTC = table.Column<DateTime>(nullable: false),
                    EndDateUTC = table.Column<DateTime>(nullable: false),
                    QuantityMember = table.Column<int>(nullable: false),
                    LimiteQuantityMember = table.Column<int>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.StockId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "Info",
                columns: table => new
                {
                    CountryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Ads",
                schema: "Action",
                columns: table => new
                {
                    AdsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OwnerId = table.Column<int>(nullable: false),
                    ContentUrl = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    QuantityViews = table.Column<int>(nullable: false),
                    CreatedDateUTC = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ads", x => x.AdsId);
                    table.ForeignKey(
                        name: "FK_Ads_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "Action",
                        principalTable: "Owners",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialNetworks",
                schema: "Action",
                columns: table => new
                {
                    SocialNetworkId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OwnerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialNetworks", x => x.SocialNetworkId);
                    table.ForeignKey(
                        name: "FK_SocialNetworks_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "Action",
                        principalTable: "Owners",
                        principalColumn: "OwnerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "Info",
                columns: table => new
                {
                    CityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    CountryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Info",
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockDetails",
                schema: "Action",
                columns: table => new
                {
                    StockDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StockId = table.Column<int>(nullable: false),
                    AdsId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Rate = table.Column<int>(nullable: false),
                    MessageHTML = table.Column<string>(nullable: true),
                    QuantityViews = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockDetails", x => x.StockDetailId);
                    table.ForeignKey(
                        name: "FK_StockDetails_Ads_AdsId",
                        column: x => x.AdsId,
                        principalSchema: "Action",
                        principalTable: "Ads",
                        principalColumn: "AdsId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockDetails_Stocks_StockId",
                        column: x => x.StockId,
                        principalSchema: "Action",
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "Info",
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockViews",
                schema: "Action",
                columns: table => new
                {
                    StockViewId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StockDetailId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    DateStartUTC = table.Column<DateTime>(nullable: false),
                    DateEndUTC = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockViews", x => x.StockViewId);
                    table.ForeignKey(
                        name: "FK_StockViews_StockDetails_StockDetailId",
                        column: x => x.StockDetailId,
                        principalSchema: "Action",
                        principalTable: "StockDetails",
                        principalColumn: "StockDetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockViews_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Winners",
                schema: "Action",
                columns: table => new
                {
                    WinnerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StockId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Place = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Winners", x => x.WinnerId);
                    table.ForeignKey(
                        name: "FK_Winners_Stocks_StockId",
                        column: x => x.StockId,
                        principalSchema: "Action",
                        principalTable: "Stocks",
                        principalColumn: "StockId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Winners_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfirmationKeys",
                schema: "Identity",
                columns: table => new
                {
                    ConfirmationKeyId = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_ConfirmationKeys", x => x.ConfirmationKeyId);
                    table.ForeignKey(
                        name: "FK_ConfirmationKeys_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                schema: "Identity",
                columns: table => new
                {
                    TokenId = table.Column<int>(nullable: false)
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
                    table.PrimaryKey("PK_Tokens", x => x.TokenId);
                    table.ForeignKey(
                        name: "FK_Tokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockViewConfirms",
                schema: "Action",
                columns: table => new
                {
                    StockViewConfirmId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StockViewId = table.Column<int>(nullable: false),
                    DateShowConfirmUTC = table.Column<DateTime>(nullable: false),
                    DateClickConfirmUTC = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockViewConfirms", x => x.StockViewConfirmId);
                    table.ForeignKey(
                        name: "FK_StockViewConfirms_StockViews_StockViewId",
                        column: x => x.StockViewId,
                        principalSchema: "Action",
                        principalTable: "StockViews",
                        principalColumn: "StockViewId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockViewDetails",
                schema: "Action",
                columns: table => new
                {
                    StockViewDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StockViewId = table.Column<int>(nullable: false),
                    TimeLine = table.Column<int>(nullable: false),
                    DateFixationUTC = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockViewDetails", x => x.StockViewDetailId);
                    table.ForeignKey(
                        name: "FK_StockViewDetails_StockViews_StockViewId",
                        column: x => x.StockViewId,
                        principalSchema: "Action",
                        principalTable: "StockViews",
                        principalColumn: "StockViewId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Info",
                table: "Countries",
                columns: new[] { "CountryId", "Name" },
                values: new object[] { 1, "Казахстан" });

            migrationBuilder.InsertData(
                schema: "Info",
                table: "Cities",
                columns: new[] { "CityId", "CountryId", "Name" },
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
                columns: new[] { "UserId", "ActivatedDateUtc", "Birthday", "CityId", "CreatedDateUtc", "Email", "EmailIsConfirmed", "EmailIsSubscribe", "EmailKeyUnSubscribe", "FathersName", "IsActive", "Login", "Name", "Password", "PhoneIsConfirmed", "PhoneNumber", "Salt", "SurName" },
                values: new object[] { 1, new DateTime(2019, 5, 7, 10, 4, 22, 278, DateTimeKind.Utc), new DateTime(1992, 5, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2019, 5, 21, 10, 48, 34, 279, DateTimeKind.Utc), "admin@google.com", false, false, null, "Andreevich", true, "vektor", "Viktor", "IEF42EQpZswVSldQapwVnv6U4z8BoEvysfXSdUto6d0=", true, "7771291221", "Xvot2f0+bUcAjyRkWKdwmQ==", "Bochikalov" });

            migrationBuilder.InsertData(
                schema: "Identity",
                table: "Users",
                columns: new[] { "UserId", "ActivatedDateUtc", "Birthday", "CityId", "CreatedDateUtc", "Email", "EmailIsConfirmed", "EmailIsSubscribe", "EmailKeyUnSubscribe", "FathersName", "IsActive", "Login", "Name", "Password", "PhoneIsConfirmed", "PhoneNumber", "Salt", "SurName" },
                values: new object[] { 2, null, null, 1, new DateTime(2019, 5, 21, 10, 48, 34, 281, DateTimeKind.Utc), null, false, false, null, null, true, null, "Levon", "yT9q/QmEkRh/qnG9Qn3sLjRhNnmRTWhe/tdkuAXyn08=", false, "7771940504", "K9kgjCuNehdq6BNv+G5obg==", "Kukuyan" });

            migrationBuilder.CreateIndex(
                name: "IX_Ads_OwnerId",
                schema: "Action",
                table: "Ads",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialNetworks_OwnerId",
                schema: "Action",
                table: "SocialNetworks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_AdsId",
                schema: "Action",
                table: "StockDetails",
                column: "AdsId");

            migrationBuilder.CreateIndex(
                name: "IX_StockDetails_StockId",
                schema: "Action",
                table: "StockDetails",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_StockViewConfirms_StockViewId",
                schema: "Action",
                table: "StockViewConfirms",
                column: "StockViewId");

            migrationBuilder.CreateIndex(
                name: "IX_StockViewDetails_StockViewId",
                schema: "Action",
                table: "StockViewDetails",
                column: "StockViewId");

            migrationBuilder.CreateIndex(
                name: "IX_StockViews_StockDetailId",
                schema: "Action",
                table: "StockViews",
                column: "StockDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_StockViews_UserId",
                schema: "Action",
                table: "StockViews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Winners_StockId",
                schema: "Action",
                table: "Winners",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Winners_UserId",
                schema: "Action",
                table: "Winners",
                column: "UserId");

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
                name: "SocialNetworks",
                schema: "Action");

            migrationBuilder.DropTable(
                name: "StockViewConfirms",
                schema: "Action");

            migrationBuilder.DropTable(
                name: "StockViewDetails",
                schema: "Action");

            migrationBuilder.DropTable(
                name: "Winners",
                schema: "Action");

            migrationBuilder.DropTable(
                name: "ConfirmationKeys",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Tokens",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "StockViews",
                schema: "Action");

            migrationBuilder.DropTable(
                name: "StockDetails",
                schema: "Action");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Ads",
                schema: "Action");

            migrationBuilder.DropTable(
                name: "Stocks",
                schema: "Action");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "Info");

            migrationBuilder.DropTable(
                name: "Owners",
                schema: "Action");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "Info");
        }
    }
}
