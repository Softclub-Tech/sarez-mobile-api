using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBrandCatalogCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Colors_ColorName",
                table: "Colors",
                column: "ColorName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName_CatalogId",
                table: "Categories",
                columns: new[] { "CategoryName", "CatalogId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_CatalogName",
                table: "Catalogs",
                column: "CatalogName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_BrandName",
                table: "Brands",
                column: "BrandName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Colors_ColorName",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategoryName_CatalogId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_CatalogName",
                table: "Catalogs");

            migrationBuilder.DropIndex(
                name: "IX_Brands_BrandName",
                table: "Brands");
        }
    }
}
