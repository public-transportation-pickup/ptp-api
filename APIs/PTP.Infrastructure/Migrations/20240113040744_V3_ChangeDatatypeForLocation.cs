using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V3_ChangeDatatypeForLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_User_UserId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("67091765-fd79-4493-b9d8-8f0c8c5c9470"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c2e09ab9-ec52-4497-b337-56261bc14014"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c7d2bba4-0487-4f88-90b9-7966161d5528"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("cf2febc3-4a55-4d8e-b7b2-ee3a67883e8d"));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Wallet",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "Wallet",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WalletType",
                table: "Wallet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "User",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "User",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<string>(
                name: "FCMToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transaction",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Store",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Store",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Station",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Station",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "AverageVelocity",
                table: "RouteVars",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "AverageVelocity",
                table: "Route",
                type: "float(18)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("4a09fd7e-8818-4a02-81df-8b3a75a90b57"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 13, 11, 7, 44, 805, DateTimeKind.Local).AddTicks(6577), false, null, null, "Customer" },
                    { new Guid("72cce70a-9124-4d94-a278-f4993e8983b2"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 13, 11, 7, 44, 805, DateTimeKind.Local).AddTicks(6621), false, null, null, "Admin" },
                    { new Guid("7fe9b7dd-5461-4d18-8d7e-9836e881ec12"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 13, 11, 7, 44, 805, DateTimeKind.Local).AddTicks(6555), false, null, null, "StoreManager" },
                    { new Guid("bf286d85-b289-4344-a20b-c26858acf844"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 13, 11, 7, 44, 805, DateTimeKind.Local).AddTicks(6623), false, null, null, "TransportationEmployee" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_StoreId",
                table: "Wallet",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_Store_StoreId",
                table: "Wallet",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_User_UserId",
                table: "Wallet",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_Store_StoreId",
                table: "Wallet");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_User_UserId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_StoreId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("4a09fd7e-8818-4a02-81df-8b3a75a90b57"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("72cce70a-9124-4d94-a278-f4993e8983b2"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("7fe9b7dd-5461-4d18-8d7e-9836e881ec12"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("bf286d85-b289-4344-a20b-c26858acf844"));

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "WalletType",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "FCMToken",
                table: "User");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "Wallet",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "User",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "User",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Transaction",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Store",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Store",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Station",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Station",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldPrecision: 18,
                oldScale: 10);

            migrationBuilder.AlterColumn<double>(
                name: "AverageVelocity",
                table: "RouteVars",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "AverageVelocity",
                table: "Route",
                type: "float",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(18)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("67091765-fd79-4493-b9d8-8f0c8c5c9470"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 21, 9, 53, 93, DateTimeKind.Local).AddTicks(2500), false, null, null, "TransportationEmployee" },
                    { new Guid("c2e09ab9-ec52-4497-b337-56261bc14014"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 21, 9, 53, 93, DateTimeKind.Local).AddTicks(2474), false, null, null, "StoreManager" },
                    { new Guid("c7d2bba4-0487-4f88-90b9-7966161d5528"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 21, 9, 53, 93, DateTimeKind.Local).AddTicks(2498), false, null, null, "Admin" },
                    { new Guid("cf2febc3-4a55-4d8e-b7b2-ee3a67883e8d"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 21, 9, 53, 93, DateTimeKind.Local).AddTicks(2494), false, null, null, "Customer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_UserId",
                table: "Wallet",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_User_UserId",
                table: "Wallet",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
