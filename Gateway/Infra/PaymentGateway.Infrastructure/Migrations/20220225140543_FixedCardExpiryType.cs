using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Infrastructure.Migrations
{
    public partial class FixedCardExpiryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Gateway",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2022, 2, 25, 14, 5, 42, 632, DateTimeKind.Local).AddTicks(3106),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2022, 2, 23, 21, 3, 50, 104, DateTimeKind.Local).AddTicks(9912));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CardExpiry",
                schema: "Gateway",
                table: "Payment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                oldDefaultValue: new DateTime(2022, 2, 25, 14, 5, 42, 632, DateTimeKind.Local).AddTicks(3106));

            migrationBuilder.AlterColumn<string>(
                name: "CardExpiry",
                schema: "Gateway",
                table: "Payment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
