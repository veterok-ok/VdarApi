using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VdarApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tokens",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    client_id = table.Column<string>(nullable: false),
                    access_token = table.Column<string>(nullable: false),
                    refresh_token = table.Column<string>(nullable: false),
                    update_hash_sum = table.Column<string>(nullable: false),
                    finger_print = table.Column<string>(nullable: false),
                    created_date_utc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tokens", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tokens");
        }
    }
}
