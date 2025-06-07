using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTFGym.Data.Migrations
{
    /// <inheritdoc />
    public partial class NoviFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TerminClans");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TerminClans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClanId = table.Column<int>(type: "int", nullable: false),
                    TerminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminClans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerminClans_Clan_ClanId",
                        column: x => x.ClanId,
                        principalTable: "Clan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TerminClans_Termin_TerminId",
                        column: x => x.TerminId,
                        principalTable: "Termin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TerminClans_ClanId",
                table: "TerminClans",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminClans_TerminId",
                table: "TerminClans",
                column: "TerminId");
        }
    }
}
