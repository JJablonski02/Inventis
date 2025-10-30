using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventis.Infrastructure.Postgres.Migrations;

/// <inheritdoc />
public partial class AddedInventoryMovementLogs : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "VatRate",
            schema: "inventis",
            table: "Products",
            newName: "PurchasePriceVatRate");

        migrationBuilder.RenameColumn(
            name: "QuantityInWarehouse",
            schema: "inventis",
            table: "Products",
            newName: "StoredQuantityInWarehouse");

        migrationBuilder.RenameColumn(
            name: "QuantityInStore",
            schema: "inventis",
            table: "Products",
            newName: "StoredQuantityInStore");

        migrationBuilder.RenameColumn(
            name: "QuantityInBackroom",
            schema: "inventis",
            table: "Products",
            newName: "StoredQuantityInBackroom");

        migrationBuilder.AddColumn<decimal>(
            name: "CurrentQuantityInBackroom",
            schema: "inventis",
            table: "Products",
            type: "numeric",
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<decimal>(
            name: "CurrentQuantityInStore",
            schema: "inventis",
            table: "Products",
            type: "numeric",
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<decimal>(
            name: "CurrentQuantityInWarehouse",
            schema: "inventis",
            table: "Products",
            type: "numeric",
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<decimal>(
            name: "SalePriceVatRate",
            schema: "inventis",
            table: "Products",
            type: "numeric",
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.CreateTable(
            name: "InventoryMovementLogs",
            schema: "inventis",
            columns: table => new
            {
                Id = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                ProductId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: false),
                ScanId = table.Column<string>(type: "character varying(26)", maxLength: 26, nullable: true),
                Action = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                Direction = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                CurrentQuantityInStoreBefore = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                CurrentQuantityInBackroomBefore = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                CurrentQuantityInWarehouseBefore = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                CurrentQuantityInStoreAfter = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                CurrentQuantityInBackroomAfter = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                CurrentQuantityInWarehouseAfter = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                StoredQuantityInStoreBefore = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                StoredQuantityInBackroomBefore = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                StoredQuantityInWarehouseBefore = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                StoredQuantityInStoreAfter = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                StoredQuantityInBackroomAfter = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                StoredQuantityInWarehouseAfter = table.Column<decimal>(type: "numeric(8,2)", precision: 8, scale: 2, nullable: false),
                Version = table.Column<long>(type: "bigint", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_InventoryMovementLogs", x => x.Id);
                table.ForeignKey(
                    name: "FK_InventoryMovementLogs_Products_ProductId",
                    column: x => x.ProductId,
                    principalSchema: "inventis",
                    principalTable: "Products",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_InventoryMovementLogs_ProductId",
            schema: "inventis",
            table: "InventoryMovementLogs",
            column: "ProductId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "InventoryMovementLogs",
            schema: "inventis");

        migrationBuilder.DropColumn(
            name: "CurrentQuantityInBackroom",
            schema: "inventis",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "CurrentQuantityInStore",
            schema: "inventis",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "CurrentQuantityInWarehouse",
            schema: "inventis",
            table: "Products");

        migrationBuilder.DropColumn(
            name: "SalePriceVatRate",
            schema: "inventis",
            table: "Products");

        migrationBuilder.RenameColumn(
            name: "StoredQuantityInWarehouse",
            schema: "inventis",
            table: "Products",
            newName: "QuantityInWarehouse");

        migrationBuilder.RenameColumn(
            name: "StoredQuantityInStore",
            schema: "inventis",
            table: "Products",
            newName: "QuantityInStore");

        migrationBuilder.RenameColumn(
            name: "StoredQuantityInBackroom",
            schema: "inventis",
            table: "Products",
            newName: "QuantityInBackroom");

        migrationBuilder.RenameColumn(
            name: "PurchasePriceVatRate",
            schema: "inventis",
            table: "Products",
            newName: "VatRate");
    }
}
