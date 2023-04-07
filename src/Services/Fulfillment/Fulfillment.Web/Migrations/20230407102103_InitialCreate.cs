using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Fulfillment.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Buyers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PersonalInformation_FirstName = table.Column<string>(type: "text", nullable: false),
                    PersonalInformation_LastName = table.Column<string>(type: "text", nullable: false),
                    PersonalInformation_Email = table.Column<string>(type: "text", nullable: false),
                    ShippingAddress_Street = table.Column<string>(type: "text", nullable: false),
                    ShippingAddress_Zip = table.Column<string>(type: "text", nullable: false),
                    ShippingAddress_City = table.Column<string>(type: "text", nullable: false),
                    ShippingAddress_Country = table.Column<string>(type: "text", nullable: false),
                    PaymentInformation_Address_Street = table.Column<string>(type: "text", nullable: true),
                    PaymentInformation_Address_Zip = table.Column<string>(type: "text", nullable: true),
                    PaymentInformation_Address_City = table.Column<string>(type: "text", nullable: true),
                    PaymentInformation_Address_Country = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TotalPrice_GrossPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice_NetPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice_Tax = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice_Currency = table.Column<string>(type: "text", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price_GrossPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Price_NetPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Price_Tax = table.Column<decimal>(type: "numeric", nullable: false),
                    Price_Currency = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    TotalPrice_GrossPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice_NetPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice_Tax = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPrice_Currency = table.Column<string>(type: "text", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItem_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                table: "OrderItem",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Buyers");

            migrationBuilder.DropTable(
                name: "OrderItem");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
