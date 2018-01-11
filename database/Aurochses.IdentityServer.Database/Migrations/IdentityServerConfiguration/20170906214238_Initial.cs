using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Aurochses.IdentityServer.Database.Migrations.IdentityServerConfiguration
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                "identityServer");

            migrationBuilder.CreateTable(
                "ApiResources",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Enabled = table.Column<bool>(),
                    Name = table.Column<string>(maxLength: 200)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "Clients",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AbsoluteRefreshTokenLifetime = table.Column<int>(),
                    AccessTokenLifetime = table.Column<int>(),
                    AccessTokenType = table.Column<int>(),
                    AllowAccessTokensViaBrowser = table.Column<bool>(),
                    AllowOfflineAccess = table.Column<bool>(),
                    AllowPlainTextPkce = table.Column<bool>(),
                    AllowRememberConsent = table.Column<bool>(),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(),
                    AlwaysSendClientClaims = table.Column<bool>(),
                    AuthorizationCodeLifetime = table.Column<int>(),
                    ClientId = table.Column<string>(maxLength: 200),
                    ClientName = table.Column<string>(maxLength: 200, nullable: true),
                    ClientUri = table.Column<string>(maxLength: 2000, nullable: true),
                    EnableLocalLogin = table.Column<bool>(),
                    Enabled = table.Column<bool>(),
                    IdentityTokenLifetime = table.Column<int>(),
                    IncludeJwtId = table.Column<bool>(),
                    LogoUri = table.Column<string>(nullable: true),
                    LogoutSessionRequired = table.Column<bool>(),
                    LogoutUri = table.Column<string>(nullable: true),
                    PrefixClientClaims = table.Column<bool>(),
                    ProtocolType = table.Column<string>(maxLength: 200),
                    RefreshTokenExpiration = table.Column<int>(),
                    RefreshTokenUsage = table.Column<int>(),
                    RequireClientSecret = table.Column<bool>(),
                    RequireConsent = table.Column<bool>(),
                    RequirePkce = table.Column<bool>(),
                    SlidingRefreshTokenLifetime = table.Column<int>(),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "IdentityResources",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Emphasize = table.Column<bool>(),
                    Enabled = table.Column<bool>(),
                    Name = table.Column<string>(maxLength: 200),
                    Required = table.Column<bool>(),
                    ShowInDiscoveryDocument = table.Column<bool>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                "ApiClaims",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApiResourceId = table.Column<int>(),
                    Type = table.Column<string>(maxLength: 200)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_ApiClaims_ApiResources_ApiResourceId",
                        x => x.ApiResourceId,
                        principalSchema: "identityServer",
                        principalTable: "ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ApiScopes",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApiResourceId = table.Column<int>(),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 200, nullable: true),
                    Emphasize = table.Column<bool>(),
                    Name = table.Column<string>(maxLength: 200),
                    Required = table.Column<bool>(),
                    ShowInDiscoveryDocument = table.Column<bool>()
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScopes", x => x.Id);
                    table.ForeignKey(
                        "FK_ApiScopes_ApiResources_ApiResourceId",
                        x => x.ApiResourceId,
                        principalSchema: "identityServer",
                        principalTable: "ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ApiSecrets",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApiResourceId = table.Column<int>(),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(maxLength: 250, nullable: true),
                    Value = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiSecrets", x => x.Id);
                    table.ForeignKey(
                        "FK_ApiSecrets_ApiResources_ApiResourceId",
                        x => x.ApiResourceId,
                        principalSchema: "identityServer",
                        principalTable: "ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ClientClaims",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(),
                    Type = table.Column<string>(maxLength: 250),
                    Value = table.Column<string>(maxLength: 250)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientClaims_Clients_ClientId",
                        x => x.ClientId,
                        principalSchema: "identityServer",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ClientCorsOrigins",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(),
                    Origin = table.Column<string>(maxLength: 150)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCorsOrigins", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientCorsOrigins_Clients_ClientId",
                        x => x.ClientId,
                        principalSchema: "identityServer",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ClientGrantTypes",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(),
                    GrantType = table.Column<string>(maxLength: 250)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientGrantTypes", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientGrantTypes_Clients_ClientId",
                        x => x.ClientId,
                        principalSchema: "identityServer",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ClientIdPRestrictions",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(),
                    Provider = table.Column<string>(maxLength: 200)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientIdPRestrictions", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientIdPRestrictions_Clients_ClientId",
                        x => x.ClientId,
                        principalSchema: "identityServer",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ClientPostLogoutRedirectUris",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(),
                    PostLogoutRedirectUri = table.Column<string>(maxLength: 2000)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPostLogoutRedirectUris", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientPostLogoutRedirectUris_Clients_ClientId",
                        x => x.ClientId,
                        principalSchema: "identityServer",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ClientRedirectUris",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(),
                    RedirectUri = table.Column<string>(maxLength: 2000)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRedirectUris", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientRedirectUris_Clients_ClientId",
                        x => x.ClientId,
                        principalSchema: "identityServer",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ClientScopes",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(),
                    Scope = table.Column<string>(maxLength: 200)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientScopes", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientScopes_Clients_ClientId",
                        x => x.ClientId,
                        principalSchema: "identityServer",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ClientSecrets",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(),
                    Description = table.Column<string>(maxLength: 2000, nullable: true),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Type = table.Column<string>(maxLength: 250, nullable: true),
                    Value = table.Column<string>(maxLength: 2000)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSecrets", x => x.Id);
                    table.ForeignKey(
                        "FK_ClientSecrets_Clients_ClientId",
                        x => x.ClientId,
                        principalSchema: "identityServer",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "IdentityClaims",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IdentityResourceId = table.Column<int>(),
                    Type = table.Column<string>(maxLength: 200)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_IdentityClaims_IdentityResources_IdentityResourceId",
                        x => x.IdentityResourceId,
                        principalSchema: "identityServer",
                        principalTable: "IdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "ApiScopeClaims",
                schema: "identityServer",
                columns: table => new
                {
                    Id = table.Column<int>()
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApiScopeId = table.Column<int>(),
                    Type = table.Column<string>(maxLength: 200)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScopeClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_ApiScopeClaims_ApiScopes_ApiScopeId",
                        x => x.ApiScopeId,
                        principalSchema: "identityServer",
                        principalTable: "ApiScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_ApiResources_Name",
                schema: "identityServer",
                table: "ApiResources",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApiClaims_ApiResourceId",
                schema: "identityServer",
                table: "ApiClaims",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                "IX_ApiScopes_ApiResourceId",
                schema: "identityServer",
                table: "ApiScopes",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                "IX_ApiScopes_Name",
                schema: "identityServer",
                table: "ApiScopes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ApiScopeClaims_ApiScopeId",
                schema: "identityServer",
                table: "ApiScopeClaims",
                column: "ApiScopeId");

            migrationBuilder.CreateIndex(
                "IX_ApiSecrets_ApiResourceId",
                schema: "identityServer",
                table: "ApiSecrets",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                "IX_Clients_ClientId",
                schema: "identityServer",
                table: "Clients",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                "IX_ClientClaims_ClientId",
                schema: "identityServer",
                table: "ClientClaims",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                "IX_ClientCorsOrigins_ClientId",
                schema: "identityServer",
                table: "ClientCorsOrigins",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                "IX_ClientGrantTypes_ClientId",
                schema: "identityServer",
                table: "ClientGrantTypes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                "IX_ClientIdPRestrictions_ClientId",
                schema: "identityServer",
                table: "ClientIdPRestrictions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                "IX_ClientPostLogoutRedirectUris_ClientId",
                schema: "identityServer",
                table: "ClientPostLogoutRedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                "IX_ClientRedirectUris_ClientId",
                schema: "identityServer",
                table: "ClientRedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                "IX_ClientScopes_ClientId",
                schema: "identityServer",
                table: "ClientScopes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                "IX_ClientSecrets_ClientId",
                schema: "identityServer",
                table: "ClientSecrets",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                "IX_IdentityClaims_IdentityResourceId",
                schema: "identityServer",
                table: "IdentityClaims",
                column: "IdentityResourceId");

            migrationBuilder.CreateIndex(
                "IX_IdentityResources_Name",
                schema: "identityServer",
                table: "IdentityResources",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "ApiClaims",
                "identityServer");

            migrationBuilder.DropTable(
                "ApiScopeClaims",
                "identityServer");

            migrationBuilder.DropTable(
                "ApiSecrets",
                "identityServer");

            migrationBuilder.DropTable(
                "ClientClaims",
                "identityServer");

            migrationBuilder.DropTable(
                "ClientCorsOrigins",
                "identityServer");

            migrationBuilder.DropTable(
                "ClientGrantTypes",
                "identityServer");

            migrationBuilder.DropTable(
                "ClientIdPRestrictions",
                "identityServer");

            migrationBuilder.DropTable(
                "ClientPostLogoutRedirectUris",
                "identityServer");

            migrationBuilder.DropTable(
                "ClientRedirectUris",
                "identityServer");

            migrationBuilder.DropTable(
                "ClientScopes",
                "identityServer");

            migrationBuilder.DropTable(
                "ClientSecrets",
                "identityServer");

            migrationBuilder.DropTable(
                "IdentityClaims",
                "identityServer");

            migrationBuilder.DropTable(
                "ApiScopes",
                "identityServer");

            migrationBuilder.DropTable(
                "Clients",
                "identityServer");

            migrationBuilder.DropTable(
                "IdentityResources",
                "identityServer");

            migrationBuilder.DropTable(
                "ApiResources",
                "identityServer");
        }
    }
}
