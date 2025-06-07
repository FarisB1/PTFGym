using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PTFGym.Data.Migrations
{
    /// <inheritdoc />
    public partial class Napredak : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrenerId",
                table: "Napredak",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Napredak_TrenerId",
                table: "Napredak",
                column: "TrenerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Napredak_Trener_TrenerId",
                table: "Napredak",
                column: "TrenerId",
                principalTable: "Trener",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Napredak_Trener_TrenerId",
                table: "Napredak");

            migrationBuilder.DropIndex(
                name: "IX_Napredak_TrenerId",
                table: "Napredak");

            migrationBuilder.DropColumn(
                name: "TrenerId",
                table: "Napredak");
        }
    }
}
