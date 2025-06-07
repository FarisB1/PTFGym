using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTFGym.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClanoviTermini3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClanTermin_Clan_ClanoviId",
                table: "ClanTermin");

            migrationBuilder.DropForeignKey(
                name: "FK_ClanTermin_Termin_TerminiId",
                table: "ClanTermin");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClanTermin",
                table: "ClanTermin");

            migrationBuilder.RenameTable(
                name: "ClanTermin",
                newName: "TerminClan");

            migrationBuilder.RenameIndex(
                name: "IX_ClanTermin_TerminiId",
                table: "TerminClan",
                newName: "IX_TerminClan_TerminiId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TerminClan",
                table: "TerminClan",
                columns: new[] { "ClanoviId", "TerminiId" });

            migrationBuilder.AddForeignKey(
                name: "FK_TerminClan_Clan_ClanoviId",
                table: "TerminClan",
                column: "ClanoviId",
                principalTable: "Clan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TerminClan_Termin_TerminiId",
                table: "TerminClan",
                column: "TerminiId",
                principalTable: "Termin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TerminClan_Clan_ClanoviId",
                table: "TerminClan");

            migrationBuilder.DropForeignKey(
                name: "FK_TerminClan_Termin_TerminiId",
                table: "TerminClan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TerminClan",
                table: "TerminClan");

            migrationBuilder.RenameTable(
                name: "TerminClan",
                newName: "ClanTermin");

            migrationBuilder.RenameIndex(
                name: "IX_TerminClan_TerminiId",
                table: "ClanTermin",
                newName: "IX_ClanTermin_TerminiId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClanTermin",
                table: "ClanTermin",
                columns: new[] { "ClanoviId", "TerminiId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClanTermin_Clan_ClanoviId",
                table: "ClanTermin",
                column: "ClanoviId",
                principalTable: "Clan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClanTermin_Termin_TerminiId",
                table: "ClanTermin",
                column: "TerminiId",
                principalTable: "Termin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
