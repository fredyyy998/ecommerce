using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Offering.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LocalizationCountryCode",
                table: "Offers",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Localizations",
                columns: table => new
                {
                    CountryCode = table.Column<string>(type: "varchar(255)", nullable: false),
                    CountryName = table.Column<string>(type: "longtext", nullable: false),
                    LocalName = table.Column<string>(type: "longtext", nullable: false),
                    Currency = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizations", x => x.CountryCode);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Localizations",
                columns: new[] { "CountryCode", "CountryName", "LocalName", "Currency" },
                values: new object[,]
                {
                    { "DE", "Germany", "Deutschland", "EUR" },
                    { "US", "USA", "USA", "USD" },
                    { "GB", "Great Britain", "Great Britain", "GBP" },
                    { "CH", "Switzerland", "Switzerland", "CHE" },
                });
            
            migrationBuilder.Sql(@"
                UPDATE Offers SET LocalizationCountryCode = (
                    SELECT CountryCode FROM Localizations WHERE Currency = Offers.Price_CurrencyCode
                );
            ");
            
            migrationBuilder.CreateIndex(
                name: "IX_Offers_LocalizationCountryCode",
                table: "Offers",
                column: "LocalizationCountryCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Localizations_LocalizationCountryCode",
                table: "Offers",
                column: "LocalizationCountryCode",
                principalTable: "Localizations",
                principalColumn: "CountryCode",
                onDelete: ReferentialAction.Cascade);
            
            migrationBuilder.DropColumn(
                name: "Price_CurrencyCode",
                table: "Offers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Localizations_LocalizationCountryCode",
                table: "Offers");

            migrationBuilder.DropTable(
                name: "Localizations");

            migrationBuilder.DropIndex(
                name: "IX_Offers_LocalizationCountryCode",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "LocalizationCountryCode",
                table: "Offers");

            migrationBuilder.AddColumn<string>(
                name: "Price_CurrencyCode",
                table: "Offers",
                type: "longtext",
                nullable: false);
        }
    }
}
