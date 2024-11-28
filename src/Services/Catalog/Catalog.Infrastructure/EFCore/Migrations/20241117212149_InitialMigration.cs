using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catalog.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatalogBrands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogBrands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PictureUri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CatalogTypeId = table.Column<int>(type: "int", nullable: true),
                    CatalogBrandId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CatalogTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CatalogBrands",
                columns: new[] { "Id", "BrandName" },
                values: new object[,]
                {
                    { 1, "Azure" },
                    { 2, ".NET" },
                    { 3, "Visual Studio" },
                    { 4, "SQL Server" },
                    { 5, "Other" }
                });

            migrationBuilder.InsertData(
                table: "CatalogItems",
                columns: new[] { "Id", "CatalogBrandId", "CatalogTypeId", "Description", "Name", "PictureUri", "Price" },
                values: new object[,]
                {
                    { 1, 2, 2, ".NET Bot Black Sweatshirt", ".NET Bot Black Sweatshirt", "http://catalogbaseurltobereplaced/images/products/1.png", 19.5m },
                    { 2, 2, 1, ".NET Black & White Mug", ".NET Black & White Mug", "http://catalogbaseurltobereplaced/images/products/2.png", 8.50m },
                    { 3, 5, 2, "Prism White T-Shirt", "Prism White T-Shirt", "http://catalogbaseurltobereplaced/images/products/3.png", 12m },
                    { 4, 2, 2, ".NET Foundation Sweatshirt", ".NET Foundation Sweatshirt", "http://catalogbaseurltobereplaced/images/products/4.png", 12m },
                    { 5, 5, 3, "Roslyn Red Sheet", "Roslyn Red Sheet", "http://catalogbaseurltobereplaced/images/products/5.png", 8.5m },
                    { 6, 2, 2, ".NET Blue Sweatshirt", ".NET Blue Sweatshirt", "http://catalogbaseurltobereplaced/images/products/6.png", 12m },
                    { 7, 5, 2, "Roslyn Red T-Shirt", "Roslyn Red T-Shirt", "http://catalogbaseurltobereplaced/images/products/7.png", 12m },
                    { 8, 5, 1, "Kudu Purple Sweatshirt", "Kudu Purple Sweatshirt", "http://catalogbaseurltobereplaced/images/products/8.png", 8.5m },
                    { 9, 5, 3, "Cup<T> White Mug", "Cup<T> White Mug", "http://catalogbaseurltobereplaced/images/products/9.png", 12m },
                    { 10, 2, 3, ".NET Foundation Sheet", ".NET Foundation Sheet", "http://catalogbaseurltobereplaced/images/products/10.png", 12m },
                    { 11, 2, 3, "Cup<T> Sheet", "Cup<T> Sheet", "http://catalogbaseurltobereplaced/images/products/11.png", 8.5m },
                    { 12, 5, 2, "Prism White TShirt", "Prism White TShirt", "http://catalogbaseurltobereplaced/images/products/12.png", 12m }
                });

            migrationBuilder.InsertData(
                table: "CatalogTypes",
                columns: new[] { "Id", "TypeName" },
                values: new object[,]
                {
                    { 1, "Mug" },
                    { 2, "T-Shirt" },
                    { 3, "Sheet" },
                    { 4, "USB Memory Stick" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogBrands");

            migrationBuilder.DropTable(
                name: "CatalogItems");

            migrationBuilder.DropTable(
                name: "CatalogTypes");
        }
    }
}
