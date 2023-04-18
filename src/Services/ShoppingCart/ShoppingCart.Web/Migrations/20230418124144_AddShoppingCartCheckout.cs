using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoppingCart.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddShoppingCartCheckout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartCheckout_BillingAddress_City",
                table: "ShoppingCarts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartCheckout_BillingAddress_Country",
                table: "ShoppingCarts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartCheckout_BillingAddress_Street",
                table: "ShoppingCarts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartCheckout_BillingAddress_ZipCode",
                table: "ShoppingCarts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ShoppingCartCheckout_CustomerId",
                table: "ShoppingCarts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartCheckout_Email",
                table: "ShoppingCarts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartCheckout_FirstName",
                table: "ShoppingCarts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartCheckout_LastName",
                table: "ShoppingCarts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartCheckout_ShippingAddress_City",
                table: "ShoppingCarts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartCheckout_ShippingAddress_Country",
                table: "ShoppingCarts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartCheckout_ShippingAddress_Street",
                table: "ShoppingCarts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShoppingCartCheckout_ShippingAddress_ZipCode",
                table: "ShoppingCarts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_BillingAddress_City",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_BillingAddress_Country",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_BillingAddress_Street",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_BillingAddress_ZipCode",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_CustomerId",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_Email",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_FirstName",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_LastName",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_ShippingAddress_City",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_ShippingAddress_Country",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_ShippingAddress_Street",
                table: "ShoppingCarts");

            migrationBuilder.DropColumn(
                name: "ShoppingCartCheckout_ShippingAddress_ZipCode",
                table: "ShoppingCarts");
        }
    }
}
