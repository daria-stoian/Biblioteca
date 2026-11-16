using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nume_Complet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Poza_Profil = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Autori",
                columns: table => new
                {
                    Id_Autor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume_Autor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biografie = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autori", x => x.Id_Autor);
                });

            migrationBuilder.CreateTable(
                name: "Categorii",
                columns: table => new
                {
                    Id_Categorie = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume_Categorie = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorii", x => x.Id_Categorie);
                });

            migrationBuilder.CreateTable(
                name: "Evenimente",
                columns: table => new
                {
                    Id_Eveniment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titlu_Eveniment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descriere = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Data_Ora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Locatie = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Locuri_Disponibile = table.Column<int>(type: "int", nullable: false),
                    Locuri_Rezervate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evenimente", x => x.Id_Eveniment);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contacte",
                columns: table => new
                {
                    Id_Mesaj = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizator = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Nume_Expeditor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email_Expeditor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Mesaj = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Data_Trimitere = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status_Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacte", x => x.Id_Mesaj);
                    table.ForeignKey(
                        name: "FK_Contacte_AspNetUsers_Id_Utilizator",
                        column: x => x.Id_Utilizator,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Carti",
                columns: table => new
                {
                    Id_Carte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Autor = table.Column<int>(type: "int", nullable: false),
                    Id_Categorie = table.Column<int>(type: "int", nullable: false),
                    Titlu = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    An_Publicare = table.Column<int>(type: "int", nullable: false),
                    Numar_Pagini = table.Column<int>(type: "int", nullable: false),
                    Descriere = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Imagine_Coperta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fisier_PDF = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status_Disponibilitate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carti", x => x.Id_Carte);
                    table.ForeignKey(
                        name: "FK_Carti_Autori_Id_Autor",
                        column: x => x.Id_Autor,
                        principalTable: "Autori",
                        principalColumn: "Id_Autor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carti_Categorii_Id_Categorie",
                        column: x => x.Id_Categorie,
                        principalTable: "Categorii",
                        principalColumn: "Id_Categorie",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezervari_Evenimente",
                columns: table => new
                {
                    Id_Rezervare = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizator = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Eveniment = table.Column<int>(type: "int", nullable: false),
                    Data_Rzervare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status_Rezervare = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervari_Evenimente", x => x.Id_Rezervare);
                    table.ForeignKey(
                        name: "FK_Rezervari_Evenimente_AspNetUsers_Id_Utilizator",
                        column: x => x.Id_Utilizator,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rezervari_Evenimente_Evenimente_Id_Eveniment",
                        column: x => x.Id_Eveniment,
                        principalTable: "Evenimente",
                        principalColumn: "Id_Eveniment",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imprumuturi",
                columns: table => new
                {
                    Id_Imprumut = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizator = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Carte = table.Column<int>(type: "int", nullable: false),
                    Data_Imprumut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data_Scadenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data_Returnare = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Taxa_Penalizare = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imprumuturi", x => x.Id_Imprumut);
                    table.ForeignKey(
                        name: "FK_Imprumuturi_AspNetUsers_Id_Utilizator",
                        column: x => x.Id_Utilizator,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Imprumuturi_Carti_Id_Carte",
                        column: x => x.Id_Carte,
                        principalTable: "Carti",
                        principalColumn: "Id_Carte",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lista_Dorinte",
                columns: table => new
                {
                    Id_Utilizator = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Carte = table.Column<int>(type: "int", nullable: false),
                    Data_Adaugare = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lista_Dorinte", x => new { x.Id_Utilizator, x.Id_Carte });
                    table.ForeignKey(
                        name: "FK_Lista_Dorinte_AspNetUsers_Id_Utilizator",
                        column: x => x.Id_Utilizator,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lista_Dorinte_Carti_Id_Carte",
                        column: x => x.Id_Carte,
                        principalTable: "Carti",
                        principalColumn: "Id_Carte",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recenzii",
                columns: table => new
                {
                    Id_Recenzie = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizator = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Carte = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    Comentariu = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Data_Postare = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzii", x => x.Id_Recenzie);
                    table.ForeignKey(
                        name: "FK_Recenzii_AspNetUsers_Id_Utilizator",
                        column: x => x.Id_Utilizator,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recenzii_Carti_Id_Carte",
                        column: x => x.Id_Carte,
                        principalTable: "Carti",
                        principalColumn: "Id_Carte",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Carti_Id_Autor",
                table: "Carti",
                column: "Id_Autor");

            migrationBuilder.CreateIndex(
                name: "IX_Carti_Id_Categorie",
                table: "Carti",
                column: "Id_Categorie");

            migrationBuilder.CreateIndex(
                name: "IX_Carti_ISBN",
                table: "Carti",
                column: "ISBN",
                unique: true,
                filter: "[ISBN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Contacte_Id_Utilizator",
                table: "Contacte",
                column: "Id_Utilizator");

            migrationBuilder.CreateIndex(
                name: "IX_Imprumuturi_Id_Carte",
                table: "Imprumuturi",
                column: "Id_Carte");

            migrationBuilder.CreateIndex(
                name: "IX_Imprumuturi_Id_Utilizator",
                table: "Imprumuturi",
                column: "Id_Utilizator");

            migrationBuilder.CreateIndex(
                name: "IX_Lista_Dorinte_Id_Carte",
                table: "Lista_Dorinte",
                column: "Id_Carte");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzii_Id_Carte",
                table: "Recenzii",
                column: "Id_Carte");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzii_Id_Utilizator_Id_Carte",
                table: "Recenzii",
                columns: new[] { "Id_Utilizator", "Id_Carte" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervari_Evenimente_Id_Eveniment",
                table: "Rezervari_Evenimente",
                column: "Id_Eveniment");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervari_Evenimente_Id_Utilizator",
                table: "Rezervari_Evenimente",
                column: "Id_Utilizator");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Contacte");

            migrationBuilder.DropTable(
                name: "Imprumuturi");

            migrationBuilder.DropTable(
                name: "Lista_Dorinte");

            migrationBuilder.DropTable(
                name: "Recenzii");

            migrationBuilder.DropTable(
                name: "Rezervari_Evenimente");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Carti");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Evenimente");

            migrationBuilder.DropTable(
                name: "Autori");

            migrationBuilder.DropTable(
                name: "Categorii");
        }
    }
}
