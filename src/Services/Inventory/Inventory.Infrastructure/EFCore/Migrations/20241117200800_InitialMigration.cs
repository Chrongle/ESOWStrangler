using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Inventory.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CatalogItemId = table.Column<int>(type: "int", nullable: false),
                    StockCount = table.Column<int>(type: "int", nullable: false),
                    ReservedCount = table.Column<int>(type: "int", nullable: false),
                    ShippedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "Id", "CatalogItemId", "ReservedCount", "ShippedCount", "StockCount" },
                values: new object[,]
                {
                    { 1, 1, 0, 0, 10 },
                    { 2, 2, 0, 0, 10 },
                    { 3, 3, 0, 0, 10 },
                    { 4, 4, 0, 0, 10 },
                    { 5, 5, 0, 0, 10 },
                    { 6, 6, 0, 0, 10 },
                    { 7, 7, 0, 0, 10 },
                    { 8, 8, 0, 0, 10 },
                    { 9, 9, 0, 0, 10 },
                    { 10, 10, 0, 0, 10 },
                    { 11, 11, 0, 0, 10 },
                    { 12, 12, 0, 0, 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItems");
        }
    }
}
