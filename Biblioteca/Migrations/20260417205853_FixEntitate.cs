using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Biblioteca.Migrations
{
    /// <inheritdoc />
    public partial class FixEntitate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imprumuturi_Carti_Id_Carte",
                table: "Imprumuturi");

            migrationBuilder.DropForeignKey(
                name: "FK_Imprumuturi_Utilizatori_Id_Utilizator",
                table: "Imprumuturi");

            migrationBuilder.DropForeignKey(
                name: "FK_Recenzii_Carti_Id_Carte",
                table: "Recenzii");

            migrationBuilder.DropForeignKey(
                name: "FK_Recenzii_Utilizatori_Id_Utilizator",
                table: "Recenzii");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervari_Evenimente_Utilizatori_Id_Utilizator",
                table: "Rezervari_Evenimente");

            migrationBuilder.AddForeignKey(
                name: "FK_Imprumuturi_Carti_Id_Carte",
                table: "Imprumuturi",
                column: "Id_Carte",
                principalTable: "Carti",
                principalColumn: "Id_Carte",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Imprumuturi_Utilizatori_Id_Utilizator",
                table: "Imprumuturi",
                column: "Id_Utilizator",
                principalTable: "Utilizatori",
                principalColumn: "Id_Utilizator",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recenzii_Carti_Id_Carte",
                table: "Recenzii",
                column: "Id_Carte",
                principalTable: "Carti",
                principalColumn: "Id_Carte",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recenzii_Utilizatori_Id_Utilizator",
                table: "Recenzii",
                column: "Id_Utilizator",
                principalTable: "Utilizatori",
                principalColumn: "Id_Utilizator",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervari_Evenimente_Utilizatori_Id_Utilizator",
                table: "Rezervari_Evenimente",
                column: "Id_Utilizator",
                principalTable: "Utilizatori",
                principalColumn: "Id_Utilizator",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Imprumuturi_Carti_Id_Carte",
                table: "Imprumuturi");

            migrationBuilder.DropForeignKey(
                name: "FK_Imprumuturi_Utilizatori_Id_Utilizator",
                table: "Imprumuturi");

            migrationBuilder.DropForeignKey(
                name: "FK_Recenzii_Carti_Id_Carte",
                table: "Recenzii");

            migrationBuilder.DropForeignKey(
                name: "FK_Recenzii_Utilizatori_Id_Utilizator",
                table: "Recenzii");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervari_Evenimente_Utilizatori_Id_Utilizator",
                table: "Rezervari_Evenimente");

            migrationBuilder.AddForeignKey(
                name: "FK_Imprumuturi_Carti_Id_Carte",
                table: "Imprumuturi",
                column: "Id_Carte",
                principalTable: "Carti",
                principalColumn: "Id_Carte",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Imprumuturi_Utilizatori_Id_Utilizator",
                table: "Imprumuturi",
                column: "Id_Utilizator",
                principalTable: "Utilizatori",
                principalColumn: "Id_Utilizator",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recenzii_Carti_Id_Carte",
                table: "Recenzii",
                column: "Id_Carte",
                principalTable: "Carti",
                principalColumn: "Id_Carte",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Recenzii_Utilizatori_Id_Utilizator",
                table: "Recenzii",
                column: "Id_Utilizator",
                principalTable: "Utilizatori",
                principalColumn: "Id_Utilizator",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervari_Evenimente_Utilizatori_Id_Utilizator",
                table: "Rezervari_Evenimente",
                column: "Id_Utilizator",
                principalTable: "Utilizatori",
                principalColumn: "Id_Utilizator",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
