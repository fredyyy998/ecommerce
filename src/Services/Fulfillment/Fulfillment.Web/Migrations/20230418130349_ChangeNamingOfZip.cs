using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fulfillment.Web.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNamingOfZip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShipmentAddress_Zip",
                table: "Orders",
                newName: "ShipmentAddress_ZipCode");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_Zip",
                table: "Buyers",
                newName: "ShippingAddress_ZipCode");

            migrationBuilder.RenameColumn(
                name: "PaymentInformation_Address_Zip",
                table: "Buyers",
                newName: "PaymentInformation_Address_ZipCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShipmentAddress_ZipCode",
                table: "Orders",
                newName: "ShipmentAddress_Zip");

            migrationBuilder.RenameColumn(
                name: "ShippingAddress_ZipCode",
                table: "Buyers",
                newName: "ShippingAddress_Zip");

            migrationBuilder.RenameColumn(
                name: "PaymentInformation_Address_ZipCode",
                table: "Buyers",
                newName: "PaymentInformation_Address_Zip");
        }
    }
}
