using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address_Street = table.Column<string>(type: "text", nullable: true),
                    Address_City = table.Column<string>(type: "text", nullable: true),
                    Address_Zip = table.Column<string>(type: "text", nullable: true),
                    Address_Country = table.Column<string>(type: "text", nullable: true),
                    PersonalInformation_FirstName = table.Column<string>(type: "text", nullable: true),
                    PersonalInformation_LastName = table.Column<string>(type: "text", nullable: true),
                    PersonalInformation_DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false, defaultValue: new DateOnly(1900, 1, 1)),
                    PaymentInformation_Address_Street = table.Column<string>(type: "text", nullable: true),
                    PaymentInformation_Address_City = table.Column<string>(type: "text", nullable: true),
                    PaymentInformation_Address_Zip = table.Column<string>(type: "text", nullable: true),
                    PaymentInformation_Address_Country = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password_Hash = table.Column<byte[]>(type: "bytea", nullable: false),
                    Password_Salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");
        }
    }
}
