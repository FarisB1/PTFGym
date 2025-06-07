using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTFGym.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClanoviTermini : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Termin_TerminId",
                table: "Rezervacija");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacija_TerminId",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "TerminId",
                table: "Rezervacija");

            migrationBuilder.AddColumn<int>(
                name: "TerminId",
                table: "Clan",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clan_TerminId",
                table: "Clan",
                column: "TerminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clan_Termin_TerminId",
                table: "Clan",
                column: "TerminId",
                principalTable: "Termin",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clan_Termin_TerminId",
                table: "Clan");

            migrationBuilder.DropIndex(
                name: "IX_Clan_TerminId",
                table: "Clan");

            migrationBuilder.DropColumn(
                name: "TerminId",
                table: "Clan");

            migrationBuilder.AddColumn<int>(
                name: "TerminId",
                table: "Rezervacija",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_TerminId",
                table: "Rezervacija",
                column: "TerminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Termin_TerminId",
                table: "Rezervacija",
                column: "TerminId",
                principalTable: "Termin",
                principalColumn: "Id");
        }
    }
}
