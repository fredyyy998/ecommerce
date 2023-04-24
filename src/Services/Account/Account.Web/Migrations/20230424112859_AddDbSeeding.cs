using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddDbSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Administrator",
                columns: new[] { "Id", "CreatedAt", "Email", "Role", "UpdatedAt", "Password_Hash", "Password_Salt" },
                values: new object[] { new Guid("916d53a7-bc64-4a96-b0a1-0ee5eed5d131"), new DateTime(2023, 4, 24, 13, 28, 58, 914, DateTimeKind.Local).AddTicks(2450), "ecommerce@admin.de", "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new byte[] { 214, 240, 111, 64, 207, 135, 86, 173, 95, 235, 32, 87, 92, 19, 252, 84, 219, 3, 109, 49, 39, 246, 156, 72, 60, 194, 89, 223, 170, 242, 221, 149, 46, 136, 204, 184, 77, 151, 41, 51, 59, 25, 135, 117, 118, 96, 1, 241, 87, 73, 86, 122, 192, 205, 143, 137, 162, 22, 180, 232, 252, 22, 69, 4 }, new byte[] { 153, 6, 63, 8, 7, 17, 165, 147, 177, 213, 58, 175, 193, 240, 113, 226, 196, 1, 117, 124, 239, 139, 17, 20, 248, 167, 113, 229, 23, 226, 4, 34, 220, 118, 140, 5, 96, 78, 88, 61, 17, 126, 116, 191, 155, 171, 89, 82, 0, 178, 237, 66, 133, 232, 197, 220, 102, 114, 184, 104, 144, 199, 181, 224, 226, 105, 111, 224, 192, 29, 230, 238, 206, 204, 12, 141, 94, 128, 158, 42, 127, 225, 205, 13, 232, 119, 15, 193, 46, 151, 45, 210, 190, 207, 69, 109, 171, 20, 244, 114, 240, 226, 204, 38, 181, 231, 47, 215, 225, 30, 162, 0, 95, 39, 186, 1, 217, 110, 120, 34, 237, 158, 251, 180, 108, 202, 203, 232 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Administrator",
                keyColumn: "Id",
                keyValue: new Guid("916d53a7-bc64-4a96-b0a1-0ee5eed5d131"));
        }
    }
}
