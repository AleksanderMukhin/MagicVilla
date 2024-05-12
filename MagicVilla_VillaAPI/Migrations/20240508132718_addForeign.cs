using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class addForeign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VillaId",
                table: "VillaNombers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VillaNombers_VillaId",
                table: "VillaNombers",
                column: "VillaId");

            migrationBuilder.AddForeignKey(
                name: "FK_VillaNombers_Villas_VillaId",
                table: "VillaNombers",
                column: "VillaId",
                principalTable: "Villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropForeignKey(
                name: "FK_VillaNombers_Villas_VillaId",
                table: "VillaNombers");

            migrationBuilder.DropIndex(
                name: "IX_VillaNombers_VillaId",
                table: "VillaNombers");

            migrationBuilder.DropColumn(
                name: "VillaId",
                table: "VillaNombers");
        }
    }
}
