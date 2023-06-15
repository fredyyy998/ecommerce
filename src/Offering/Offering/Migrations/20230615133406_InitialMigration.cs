using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Offering.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Price_GrossPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price_NetPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price_TaxRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price_CurrencyCode = table.Column<string>(type: "longtext", nullable: false),
                    Discount_DiscountRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Discount_StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Discount_EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OfferType = table.Column<string>(type: "longtext", nullable: false),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PackageOfferProducts",
                columns: table => new
                {
                    PackageOfferId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ProductsId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageOfferProducts", x => new { x.PackageOfferId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_PackageOfferProducts_Offers_PackageOfferId",
                        column: x => x.PackageOfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageOfferProducts_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ProductId",
                table: "Offers",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackageOfferProducts_ProductsId",
                table: "PackageOfferProducts",
                column: "ProductsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageOfferProducts");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
