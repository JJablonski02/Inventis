using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventis.Infrastructure.Postgres.Migrations;

/// <inheritdoc />
public partial class AddedQuantityTypeToProductLog : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "QuantityType",
            schema: "inventis",
            table: "InventoryMovementLogs",
            type: "character varying(15)",
            maxLength: 15,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "QuantityType",
            schema: "inventis",
            table: "InventoryMovementLogs");
    }
}
