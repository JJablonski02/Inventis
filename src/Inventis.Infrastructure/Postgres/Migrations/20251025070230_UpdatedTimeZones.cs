using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventis.Infrastructure.Postgres.Migrations;

/// <inheritdoc />
public partial class UpdatedTimeZones : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedAt",
            schema: "inventis",
            table: "Products",
            type: "timestamp without time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone");

        migrationBuilder.AlterColumn<DateTime>(
            name: "StartedAt",
            schema: "inventis",
            table: "Inventories",
            type: "timestamp without time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CompletedAt",
            schema: "inventis",
            table: "Inventories",
            type: "timestamp without time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "ScanTime",
            schema: "inventis",
            table: "DailyInventoryScans",
            type: "timestamp without time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedAt",
            schema: "inventis",
            table: "DailyInventoryReports",
            type: "timestamp without time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone");

        migrationBuilder.AlterColumn<DateTime>(
            name: "ClosedAt",
            schema: "inventis",
            table: "DailyInventoryReports",
            type: "timestamp without time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedAt",
            schema: "inventis",
            table: "Products",
            type: "timestamp with time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone");

        migrationBuilder.AlterColumn<DateTime>(
            name: "StartedAt",
            schema: "inventis",
            table: "Inventories",
            type: "timestamp with time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CompletedAt",
            schema: "inventis",
            table: "Inventories",
            type: "timestamp with time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "ScanTime",
            schema: "inventis",
            table: "DailyInventoryScans",
            type: "timestamp with time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreatedAt",
            schema: "inventis",
            table: "DailyInventoryReports",
            type: "timestamp with time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone");

        migrationBuilder.AlterColumn<DateTime>(
            name: "ClosedAt",
            schema: "inventis",
            table: "DailyInventoryReports",
            type: "timestamp with time zone",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "timestamp without time zone",
            oldNullable: true);
    }
}
