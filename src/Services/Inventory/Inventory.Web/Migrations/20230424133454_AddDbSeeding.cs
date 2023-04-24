using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDbSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Price_CurrencyCode", "Price_GrossPrice", "Price_NetPrice", "Price_SalesTax", "Description", "Name", "Stock" },
                values: new object[,]
                {
                    { new Guid("230f0201-3b1b-4d49-b2a0-85f855771865"), "EUR", 499m, 419.33m, 19, "Gaming console with 4K UHD Blu-ray drive, 825GB SSD storage	", "Sony PlayStation 5", 20 },
                    { new Guid("4566d5c6-0071-4eb2-bd45-20aa880b32e7"), "EUR", 1199m, 1007.56m, 19, "6.8-inch Dynamic AMOLED 2X, 5G capable, 256GB storage", "Samsung Galaxy S21 Ultra", 15 },
                    { new Guid("5d5385fd-5406-4a5b-a638-653cf3927349"), "EUR", 999m, 839.50m, 19, "6.1-inch Super Retina XDR display, 5G capable, 128GB storage", "iPhone 12 Pro", 25 },
                    { new Guid("641f8d19-ed9e-44e1-b635-be6d11723346"), "EUR", 1299m, 1091.60m, 19, "13-inch, 8GB RAM, 256GB SSD, 2.3GHz Dual-Core Processor", "MacBook", 10 },
                    { new Guid("e0966bcd-a21a-444b-87ea-50faf45dd25a"), "EUR", 1999m, 1679.83m, 19, "65-inch OLED display, 4K UHD, webOS 5.0, Alexa and Google Assistant compatible	", "LG OLED CX Series 65\" 4K Smart TV	", 5 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("230f0201-3b1b-4d49-b2a0-85f855771865"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("4566d5c6-0071-4eb2-bd45-20aa880b32e7"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("5d5385fd-5406-4a5b-a638-653cf3927349"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("641f8d19-ed9e-44e1-b635-be6d11723346"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("e0966bcd-a21a-444b-87ea-50faf45dd25a"));
        }
    }
}
