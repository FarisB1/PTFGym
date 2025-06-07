using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTFGym.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateKoZnaKoji2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Clan_ClanId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Trener_TrenerId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Trener_TrenerId1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClanId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TrenerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TrenerId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ClanId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TrenerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TrenerId1",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClanId = table.Column<int>(type: "int", nullable: true),
                    TrenerId = table.Column<int>(type: "int", nullable: true),
                    TrenerId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Clan_ClanId",
                        column: x => x.ClanId,
                        principalTable: "Clan",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Trener_TrenerId",
                        column: x => x.TrenerId,
                        principalTable: "Trener",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApplicationUser_Trener_TrenerId1",
                        column: x => x.TrenerId1,
                        principalTable: "Trener",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TerminClan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClanId = table.Column<int>(type: "int", nullable: false),
                    TerminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminClan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerminClan_Clan_ClanId",
                        column: x => x.ClanId,
                        principalTable: "Clan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TerminClan_Termin_TerminId",
                        column: x => x.TerminId,
                        principalTable: "Termin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_ClanId",
                table: "ApplicationUser",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_TrenerId",
                table: "ApplicationUser",
                column: "TrenerId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUser_TrenerId1",
                table: "ApplicationUser",
                column: "TrenerId1");

            migrationBuilder.CreateIndex(
                name: "IX_TerminClan_ClanId",
                table: "TerminClan",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminClan_TerminId",
                table: "TerminClan",
                column: "TerminId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "TerminClan");

            migrationBuilder.AddColumn<int>(
                name: "ClanId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TrenerId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrenerId1",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClanId",
                table: "AspNetUsers",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TrenerId",
                table: "AspNetUsers",
                column: "TrenerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TrenerId1",
                table: "AspNetUsers",
                column: "TrenerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Clan_ClanId",
                table: "AspNetUsers",
                column: "ClanId",
                principalTable: "Clan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Trener_TrenerId",
                table: "AspNetUsers",
                column: "TrenerId",
                principalTable: "Trener",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Trener_TrenerId1",
                table: "AspNetUsers",
                column: "TrenerId1",
                principalTable: "Trener",
                principalColumn: "Id");
        }
    }
}
