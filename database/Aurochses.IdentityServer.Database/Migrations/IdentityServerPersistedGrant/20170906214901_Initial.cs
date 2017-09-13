using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aurochses.IdentityServer.Database.Migrations.IdentityServerPersistedGrant
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                "identityServer");

            migrationBuilder.CreateTable(
                "PersistedGrants",
                schema: "identityServer",
                columns: table => new
                {
                    Key = table.Column<string>(maxLength: 200),
                    ClientId = table.Column<string>(maxLength: 200),
                    CreationTime = table.Column<DateTime>(),
                    Data = table.Column<string>(maxLength: 50000),
                    Expiration = table.Column<DateTime>(nullable: true),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    Type = table.Column<string>(maxLength: 50)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Key);
                });

            migrationBuilder.CreateIndex(
                "IX_PersistedGrants_SubjectId_ClientId_Type",
                schema: "identityServer",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "PersistedGrants",
                "identityServer");
        }
    }
}
