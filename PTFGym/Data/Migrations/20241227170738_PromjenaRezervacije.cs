using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTFGym.Data.Migrations
{
    /// <inheritdoc />
    public partial class PromjenaRezervacije : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Termin_TerminId",
                table: "Rezervacija");

            migrationBuilder.DropForeignKey(
                name: "FK_Termin_Trener_TrenerId",
                table: "Termin");

            migrationBuilder.AlterColumn<int>(
                name: "TrenerId",
                table: "Termin",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TerminId",
                table: "Rezervacija",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TrenerId",
                table: "Rezervacija",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_TrenerId",
                table: "Rezervacija",
                column: "TrenerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Termin_TerminId",
                table: "Rezervacija",
                column: "TerminId",
                principalTable: "Termin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Trener_TrenerId",
                table: "Rezervacija",
                column: "TrenerId",
                principalTable: "Trener",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Termin_Trener_TrenerId",
                table: "Termin",
                column: "TrenerId",
                principalTable: "Trener",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Termin_TerminId",
                table: "Rezervacija");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Trener_TrenerId",
                table: "Rezervacija");

            migrationBuilder.DropForeignKey(
                name: "FK_Termin_Trener_TrenerId",
                table: "Termin");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacija_TrenerId",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "TrenerId",
                table: "Rezervacija");

            migrationBuilder.AlterColumn<int>(
                name: "TrenerId",
                table: "Termin",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TerminId",
                table: "Rezervacija",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Termin_TerminId",
                table: "Rezervacija",
                column: "TerminId",
                principalTable: "Termin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Termin_Trener_TrenerId",
                table: "Termin",
                column: "TrenerId",
                principalTable: "Trener",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
