using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTFGym.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNekiFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TerminClan_Clan_ClanId",
                table: "TerminClan");

            migrationBuilder.DropForeignKey(
                name: "FK_TerminClan_Termin_TerminId",
                table: "TerminClan");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TerminClan",
                table: "TerminClan");

            migrationBuilder.RenameTable(
                name: "TerminClan",
                newName: "TerminClans");

            migrationBuilder.RenameIndex(
                name: "IX_TerminClan_TerminId",
                table: "TerminClans",
                newName: "IX_TerminClans_TerminId");

            migrationBuilder.RenameIndex(
                name: "IX_TerminClan_ClanId",
                table: "TerminClans",
                newName: "IX_TerminClans_ClanId");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_TerminClans",
                table: "TerminClans",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_TerminClans_Clan_ClanId",
                table: "TerminClans",
                column: "ClanId",
                principalTable: "Clan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TerminClans_Termin_TerminId",
                table: "TerminClans",
                column: "TerminId",
                principalTable: "Termin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_TerminClans_Clan_ClanId",
                table: "TerminClans");

            migrationBuilder.DropForeignKey(
                name: "FK_TerminClans_Termin_TerminId",
                table: "TerminClans");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClanId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TrenerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TrenerId1",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TerminClans",
                table: "TerminClans");

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

            migrationBuilder.RenameTable(
                name: "TerminClans",
                newName: "TerminClan");

            migrationBuilder.RenameIndex(
                name: "IX_TerminClans_TerminId",
                table: "TerminClan",
                newName: "IX_TerminClan_TerminId");

            migrationBuilder.RenameIndex(
                name: "IX_TerminClans_ClanId",
                table: "TerminClan",
                newName: "IX_TerminClan_ClanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TerminClan",
                table: "TerminClan",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_TerminClan_Clan_ClanId",
                table: "TerminClan",
                column: "ClanId",
                principalTable: "Clan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TerminClan_Termin_TerminId",
                table: "TerminClan",
                column: "TerminId",
                principalTable: "Termin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
