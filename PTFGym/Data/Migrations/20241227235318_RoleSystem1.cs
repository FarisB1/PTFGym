using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTFGym.Data.Migrations
{
    /// <inheritdoc />
    public partial class RoleSystem1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ClanId",
                table: "AspNetUsers",
                column: "ClanId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TrenerId",
                table: "AspNetUsers",
                column: "TrenerId");

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

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ClanId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TrenerId",
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
        }
    }
}
