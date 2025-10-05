using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Carts_ApplicationUserId",
                table: "Carts");

            migrationBuilder.AddCheckConstraint(
                name: "Products",
                table: "Products",
                sql: "\"DiscountPrice\" >= 0");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ApplicationUserId_ProductId",
                table: "Carts",
                columns: new[] { "ApplicationUserId", "ProductId" },
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "Carts",
                table: "Carts",
                sql: "\"Quantity\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "Products",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Carts_ApplicationUserId_ProductId",
                table: "Carts");

            migrationBuilder.DropCheckConstraint(
                name: "Carts",
                table: "Carts");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ApplicationUserId",
                table: "Carts",
                column: "ApplicationUserId");
        }
    }
}
