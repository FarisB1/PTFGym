using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTFGym.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClanoviTermini2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "ClanTermin",
                columns: table => new
                {
                    ClanoviId = table.Column<int>(type: "int", nullable: false),
                    TerminiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanTermin", x => new { x.ClanoviId, x.TerminiId });
                    table.ForeignKey(
                        name: "FK_ClanTermin_Clan_ClanoviId",
                        column: x => x.ClanoviId,
                        principalTable: "Clan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClanTermin_Termin_TerminiId",
                        column: x => x.TerminiId,
                        principalTable: "Termin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClanTermin_TerminiId",
                table: "ClanTermin",
                column: "TerminiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClanTermin");

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
    }
}
