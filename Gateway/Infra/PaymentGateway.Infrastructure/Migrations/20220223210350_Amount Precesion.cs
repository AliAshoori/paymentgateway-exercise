using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Infrastructure.Migrations
{
    public partial class AmountPrecesion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Gateway",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 23, 21, 3, 50, 104, DateTimeKind.Local).AddTicks(9912),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 23, 21, 0, 31, 66, DateTimeKind.Local).AddTicks(5447));

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "Gateway",
                table: "Payment",
                type: "decimal(18,3)",
                precision: 18,
                scale: 3,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,10)",
                oldPrecision: 12,
                oldScale: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Gateway",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 23, 21, 0, 31, 66, DateTimeKind.Local).AddTicks(5447),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 23, 21, 3, 50, 104, DateTimeKind.Local).AddTicks(9912));

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "Gateway",
                table: "Payment",
                type: "decimal(12,10)",
                precision: 12,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,3)",
                oldPrecision: 18,
                oldScale: 3);
        }
    }
}
