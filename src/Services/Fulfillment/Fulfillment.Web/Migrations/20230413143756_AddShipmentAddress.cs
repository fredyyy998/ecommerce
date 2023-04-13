using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fulfillment.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddShipmentAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShipmentAddress_City",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipmentAddress_Country",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipmentAddress_Street",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShipmentAddress_Zip",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "PersonalInformation_LastName",
                table: "Buyers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PersonalInformation_FirstName",
                table: "Buyers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShipmentAddress_City",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipmentAddress_Country",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipmentAddress_Street",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShipmentAddress_Zip",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "PersonalInformation_LastName",
                table: "Buyers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PersonalInformation_FirstName",
                table: "Buyers",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
