using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Offering.Migrations
{
    /// <inheritdoc />
    public partial class FixSingleOfferProductReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Products_ProductId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_ProductId",
                table: "Offers");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ProductId",
                table: "Offers",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Products_ProductId",
                table: "Offers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Products_ProductId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_ProductId",
                table: "Offers");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ProductId",
                table: "Offers",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Products_ProductId",
                table: "Offers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
