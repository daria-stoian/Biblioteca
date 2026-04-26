using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autori",
                columns: table => new
                {
                    Id_Autor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume_Autor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Biografie = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Nume_Categorie = table.Column<int>(type: "int", nullable: false)
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
                    Titlu_Eveniment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descriere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Ora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Locatie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Locuri_Disponibile = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evenimente", x => x.Id_Eveniment);
                });

            migrationBuilder.CreateTable(
                name: "Utilizatori",
                columns: table => new
                {
                    Id_Utilizator = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume_Complet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nume_Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parola = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<int>(type: "int", nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizatori", x => x.Id_Utilizator);
                });

            migrationBuilder.CreateTable(
                name: "Carti",
                columns: table => new
                {
                    Id_Carte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Autor = table.Column<int>(type: "int", nullable: false),
                    Id_Categorie = table.Column<int>(type: "int", nullable: false),
                    Titlu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    An_Publicare = table.Column<int>(type: "int", nullable: false),
                    Numar_Pagini = table.Column<int>(type: "int", nullable: false),
                    Descriere = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imagine_Coperta = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "Contacte",
                columns: table => new
                {
                    Id_Mesaj = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizator = table.Column<int>(type: "int", nullable: true),
                    Nume_Expeditor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email_Expeditor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Trimitere = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status_Mesaj = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacte", x => x.Id_Mesaj);
                    table.ForeignKey(
                        name: "FK_Contacte_Utilizatori_Id_Utilizator",
                        column: x => x.Id_Utilizator,
                        principalTable: "Utilizatori",
                        principalColumn: "Id_Utilizator");
                });

            migrationBuilder.CreateTable(
                name: "Rezervari_Evenimente",
                columns: table => new
                {
                    Id_Rezervare = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizator = table.Column<int>(type: "int", nullable: false),
                    Id_Eveniment = table.Column<int>(type: "int", nullable: false),
                    Data_Rzervare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status_Rezervare = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervari_Evenimente", x => x.Id_Rezervare);
                    table.ForeignKey(
                        name: "FK_Rezervari_Evenimente_Evenimente_Id_Eveniment",
                        column: x => x.Id_Eveniment,
                        principalTable: "Evenimente",
                        principalColumn: "Id_Eveniment",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezervari_Evenimente_Utilizatori_Id_Utilizator",
                        column: x => x.Id_Utilizator,
                        principalTable: "Utilizatori",
                        principalColumn: "Id_Utilizator",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Imprumuturi",
                columns: table => new
                {
                    Id_Imprumut = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizator = table.Column<int>(type: "int", nullable: false),
                    Id_Carte = table.Column<int>(type: "int", nullable: false),
                    Data_Imprumut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data_Scadenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data_Returnare = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Taxa_Penalizare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imprumuturi", x => x.Id_Imprumut);
                    table.ForeignKey(
                        name: "FK_Imprumuturi_Carti_Id_Carte",
                        column: x => x.Id_Carte,
                        principalTable: "Carti",
                        principalColumn: "Id_Carte",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Imprumuturi_Utilizatori_Id_Utilizator",
                        column: x => x.Id_Utilizator,
                        principalTable: "Utilizatori",
                        principalColumn: "Id_Utilizator",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lista_Dorinte",
                columns: table => new
                {
                    Id_Utilizator = table.Column<int>(type: "int", nullable: false),
                    Id_Carte = table.Column<int>(type: "int", nullable: false),
                    Data_Adaugare = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lista_Dorinte", x => new { x.Id_Utilizator, x.Id_Carte });
                    table.ForeignKey(
                        name: "FK_Lista_Dorinte_Carti_Id_Carte",
                        column: x => x.Id_Carte,
                        principalTable: "Carti",
                        principalColumn: "Id_Carte",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lista_Dorinte_Utilizatori_Id_Utilizator",
                        column: x => x.Id_Utilizator,
                        principalTable: "Utilizatori",
                        principalColumn: "Id_Utilizator",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recenzii",
                columns: table => new
                {
                    Id_Recenzie = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizator = table.Column<int>(type: "int", nullable: false),
                    Id_Carte = table.Column<int>(type: "int", nullable: false),
                    Nota = table.Column<int>(type: "int", nullable: false),
                    Comentariu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Postare = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzii", x => x.Id_Recenzie);
                    table.ForeignKey(
                        name: "FK_Recenzii_Carti_Id_Carte",
                        column: x => x.Id_Carte,
                        principalTable: "Carti",
                        principalColumn: "Id_Carte",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Recenzii_Utilizatori_Id_Utilizator",
                        column: x => x.Id_Utilizator,
                        principalTable: "Utilizatori",
                        principalColumn: "Id_Utilizator",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carti_Id_Autor",
                table: "Carti",
                column: "Id_Autor");

            migrationBuilder.CreateIndex(
                name: "IX_Carti_Id_Categorie",
                table: "Carti",
                column: "Id_Categorie");

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
                name: "IX_Recenzii_Id_Utilizator",
                table: "Recenzii",
                column: "Id_Utilizator");

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
                name: "Carti");

            migrationBuilder.DropTable(
                name: "Evenimente");

            migrationBuilder.DropTable(
                name: "Utilizatori");

            migrationBuilder.DropTable(
                name: "Autori");

            migrationBuilder.DropTable(
                name: "Categorii");
        }
    }
}
