using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventis.Infrastructure.Postgres.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "inventis");

        migrationBuilder.CreateTable(
            name: "DailyInventoryReports",
            schema: "inventis",
            columns: table => new
            {
                Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                IsClosed = table.Column<bool>(type: "boolean", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                Version = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DailyInventoryReports", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Inventories",
            schema: "inventis",
            columns: table => new
            {
                Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                UserId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                UserFullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                Version = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Inventories", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Products",
            schema: "inventis",
            columns: table => new
            {
                Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                EanCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                NetPurchasePrice = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                GrossPurchasePrice = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                NetSalePrice = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                GrossSalePrice = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                TotalPurchaseGrossValue = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                TotalSaleGrossValue = table.Column<decimal>(type: "numeric(16,2)", precision: 16, scale: 2, nullable: false),
                QuantityInBackroom = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                QuantityInWarehouse = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                QuantityInStore = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                VatRate = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                ProviderName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                ProviderContactDetails = table.Column<string>(type: "text", nullable: true),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            schema: "inventis",
            columns: table => new
            {
                Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "DailyInventoryScans",
            schema: "inventis",
            columns: table => new
            {
                Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                ProductId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                Note = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                ScanTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                DailyInventoryReportId = table.Column<string>(type: "character varying(26)", nullable: false),
                xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DailyInventoryScans", x => x.Id);
                table.ForeignKey(
                    name: "FK_DailyInventoryScans_DailyInventoryReports_DailyInventoryRep~",
                    column: x => x.DailyInventoryReportId,
                    principalSchema: "inventis",
                    principalTable: "DailyInventoryReports",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "InventoryItems",
            schema: "inventis",
            columns: table => new
            {
                Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                ProductId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                ProductName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Quantity = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                ExpectedQuantity = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                InventoryId = table.Column<string>(type: "character varying(26)", nullable: false),
                xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InventoryItems", x => x.Id);
                table.ForeignKey(
                    name: "FK_InventoryItems_Inventories_InventoryId",
                    column: x => x.InventoryId,
                    principalSchema: "inventis",
                    principalTable: "Inventories",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DailyInventoryScans_DailyInventoryReportId",
            schema: "inventis",
            table: "DailyInventoryScans",
            column: "DailyInventoryReportId");

        migrationBuilder.CreateIndex(
            name: "IX_InventoryItems_InventoryId",
            schema: "inventis",
            table: "InventoryItems",
            column: "InventoryId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DailyInventoryScans",
            schema: "inventis");

        migrationBuilder.DropTable(
            name: "InventoryItems",
            schema: "inventis");

        migrationBuilder.DropTable(
            name: "Products",
            schema: "inventis");

        migrationBuilder.DropTable(
            name: "Users",
            schema: "inventis");

        migrationBuilder.DropTable(
            name: "DailyInventoryReports",
            schema: "inventis");

        migrationBuilder.DropTable(
            name: "Inventories",
            schema: "inventis");
    }
}
