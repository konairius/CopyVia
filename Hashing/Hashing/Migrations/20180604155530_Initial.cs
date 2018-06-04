using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hashing.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HashInfos",
                columns: table => new
                {
                    HashId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Algorithm = table.Column<string>(nullable: false),
                    Size = table.Column<uint>(nullable: false),
                    MTime = table.Column<DateTime>(nullable: false),
                    PathHash = table.Column<byte[]>(nullable: false),
                    Hash = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HashInfos", x => x.HashId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HashInfos");
        }
    }
}
