using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aurochses.IdentityServer.Database.Migrations.IdentityServerConfiguration
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoutUri",
                schema: "identityServer",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "PrefixClientClaims",
                schema: "identityServer",
                table: "Clients",
                newName: "FrontChannelLogoutSessionRequired");

            migrationBuilder.RenameColumn(
                name: "LogoutSessionRequired",
                schema: "identityServer",
                table: "Clients",
                newName: "BackChannelLogoutSessionRequired");

            migrationBuilder.AlterColumn<string>(
                name: "LogoUri",
                schema: "identityServer",
                table: "Clients",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackChannelLogoutUri",
                schema: "identityServer",
                table: "Clients",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientClaimsPrefix",
                schema: "identityServer",
                table: "Clients",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConsentLifetime",
                schema: "identityServer",
                table: "Clients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "identityServer",
                table: "Clients",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FrontChannelLogoutUri",
                schema: "identityServer",
                table: "Clients",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PairWiseSubjectSalt",
                schema: "identityServer",
                table: "Clients",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientProperties",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(),
                    Key = table.Column<string>(maxLength: 250),
                    Value = table.Column<string>(maxLength: 2000)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientProperties_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "identityServer",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientProperties_ClientId",
                schema: "identityServer",
                table: "ClientProperties",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientProperties",
                schema: "identityServer");

            migrationBuilder.DropColumn(
                name: "BackChannelLogoutUri",
                schema: "identityServer",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ClientClaimsPrefix",
                schema: "identityServer",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ConsentLifetime",
                schema: "identityServer",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "identityServer",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "FrontChannelLogoutUri",
                schema: "identityServer",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PairWiseSubjectSalt",
                schema: "identityServer",
                table: "Clients");

            migrationBuilder.RenameColumn(
                name: "FrontChannelLogoutSessionRequired",
                schema: "identityServer",
                table: "Clients",
                newName: "PrefixClientClaims");

            migrationBuilder.RenameColumn(
                name: "BackChannelLogoutSessionRequired",
                schema: "identityServer",
                table: "Clients",
                newName: "LogoutSessionRequired");

            migrationBuilder.AlterColumn<string>(
                name: "LogoUri",
                schema: "identityServer",
                table: "Clients",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoutUri",
                schema: "identityServer",
                table: "Clients",
                nullable: true);
        }
    }
}
