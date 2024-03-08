using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V3_01_Update_Menu_StoreUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallet_Store_StoreId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_StoreId",
                table: "Wallet");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2a7ef74e-075e-4b3c-ba23-f068915de9d3"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2a8d9301-730d-41b3-924b-635cf1d1adaf"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("674638de-47f2-495f-8c01-5394ba9391a7"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("dd58a7b4-6b27-45b2-aa4a-5d1940ce78ff"));

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Wallet");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "QuantityInDay",
                table: "ProductInMenu");

            migrationBuilder.DropColumn(
                name: "DateFilter",
                table: "Menu");

            migrationBuilder.RenameColumn(
                name: "ActualPrice",
                table: "ProductInMenu",
                newName: "SalePrice");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateApply",
                table: "Menu",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("2fa9de53-d895-4435-986f-0cdf8d099910"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 8, 21, 57, 33, 292, DateTimeKind.Local).AddTicks(4348), false, null, null, "Admin" },
                    { new Guid("4749a8a8-28a0-4a69-a484-88634e20b514"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 8, 21, 57, 33, 292, DateTimeKind.Local).AddTicks(4345), false, null, null, "Customer" },
                    { new Guid("dab012ba-9a4d-409e-8634-af869b8e0138"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 8, 21, 57, 33, 292, DateTimeKind.Local).AddTicks(4318), false, null, null, "StoreManager" },
                    { new Guid("ead1e5c0-8321-4b40-9d7b-19fc95711fc2"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 8, 21, 57, 33, 292, DateTimeKind.Local).AddTicks(4351), false, null, null, "TransportationEmployee" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2fa9de53-d895-4435-986f-0cdf8d099910"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("4749a8a8-28a0-4a69-a484-88634e20b514"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("dab012ba-9a4d-409e-8634-af869b8e0138"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("ead1e5c0-8321-4b40-9d7b-19fc95711fc2"));

            migrationBuilder.DropColumn(
                name: "DateApply",
                table: "Menu");

            migrationBuilder.RenameColumn(
                name: "SalePrice",
                table: "ProductInMenu",
                newName: "ActualPrice");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "Wallet",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WalletId",
                table: "Store",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantityInDay",
                table: "ProductInMenu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DateFilter",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("2a7ef74e-075e-4b3c-ba23-f068915de9d3"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 29, 12, 43, 38, 472, DateTimeKind.Local).AddTicks(824), false, null, null, "StoreManager" },
                    { new Guid("2a8d9301-730d-41b3-924b-635cf1d1adaf"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 29, 12, 43, 38, 472, DateTimeKind.Local).AddTicks(865), false, null, null, "TransportationEmployee" },
                    { new Guid("674638de-47f2-495f-8c01-5394ba9391a7"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 29, 12, 43, 38, 472, DateTimeKind.Local).AddTicks(859), false, null, null, "Customer" },
                    { new Guid("dd58a7b4-6b27-45b2-aa4a-5d1940ce78ff"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 29, 12, 43, 38, 472, DateTimeKind.Local).AddTicks(862), false, null, null, "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_StoreId",
                table: "Wallet",
                column: "StoreId",
                unique: true,
                filter: "[StoreId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallet_Store_StoreId",
                table: "Wallet",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");
        }
    }
}
