using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCatalogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Catalogs_CatalogId",
                table: "Categories");

            migrationBuilder.DropTable(
                name: "Catalogs");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CatalogId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategoryName_CatalogId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CatalogId",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_SubCategoryName_CategoryId",
                table: "SubCategories",
                columns: new[] { "SubCategoryName", "CategoryId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories",
                column: "CategoryName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SubCategories_SubCategoryName_CategoryId",
                table: "SubCategories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_CategoryName",
                table: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "CatalogId",
                table: "Categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Catalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CatalogName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_CatalogId",
                table: "Categories",
                column: "CatalogId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Catalogs_CatalogId",
                table: "Categories",
                column: "CatalogId",
                principalTable: "Catalogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
