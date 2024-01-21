using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V5_03_UpdateStore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductInMenu_ProductInMenuId",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_StoreId",
                table: "Wallet");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("1d8ca625-bb3c-42dc-8fdb-2372928c4c72"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("201319ab-0d25-4795-92b8-7ffa2f998169"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3b1ccf79-0768-438c-8836-63d74559fe64"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("5f676b28-fda8-462b-8e32-eed53dc07f01"));

            migrationBuilder.DropColumn(
                name: "CommissionRate",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "ProductInMenuId",
                table: "OrderDetails",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductInMenuId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductId");

            migrationBuilder.AlterColumn<string>(
                name: "FCMToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "User",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "User",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreCode",
                table: "Store",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Store",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WalletId",
                table: "Store",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PreparationTime",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("39633f15-0eee-4819-8440-c895fde4457e"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 22, 0, 35, 0, 409, DateTimeKind.Local).AddTicks(4610), false, null, null, "TransportationEmployee" },
                    { new Guid("49717bcb-030f-4c25-ba63-3e5b940a7ea5"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 22, 0, 35, 0, 409, DateTimeKind.Local).AddTicks(4605), false, null, null, "Customer" },
                    { new Guid("b6bf29a8-c93e-4225-808d-b003a7ed0a49"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 22, 0, 35, 0, 409, DateTimeKind.Local).AddTicks(4579), false, null, null, "StoreManager" },
                    { new Guid("c95c41cc-f90a-4da0-be77-ad7b40643de6"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 22, 0, 35, 0, 409, DateTimeKind.Local).AddTicks(4608), false, null, null, "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_StoreId",
                table: "Wallet",
                column: "StoreId",
                unique: true,
                filter: "[StoreId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_User_StoreId",
                table: "User",
                column: "StoreId",
                unique: true,
                filter: "[StoreId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Order_StoreId",
                table: "Order",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Store_StoreId",
                table: "Order",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Store_StoreId",
                table: "User",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Store_StoreId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Store_StoreId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Wallet_StoreId",
                table: "Wallet");

            migrationBuilder.DropIndex(
                name: "IX_User_StoreId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Order_StoreId",
                table: "Order");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("39633f15-0eee-4819-8440-c895fde4457e"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("49717bcb-030f-4c25-ba63-3e5b940a7ea5"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b6bf29a8-c93e-4225-808d-b003a7ed0a49"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c95c41cc-f90a-4da0-be77-ad7b40643de6"));

            migrationBuilder.DropColumn(
                name: "Password",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "StoreCode",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "WalletId",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PreparationTime",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Category");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderDetails",
                newName: "ProductInMenuId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductInMenuId");

            migrationBuilder.AlterColumn<string>(
                name: "FCMToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "User",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionRate",
                table: "Order",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageURL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImage_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("1d8ca625-bb3c-42dc-8fdb-2372928c4c72"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 15, 12, 51, 9, 946, DateTimeKind.Local).AddTicks(5278), false, null, null, "StoreManager" },
                    { new Guid("201319ab-0d25-4795-92b8-7ffa2f998169"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 15, 12, 51, 9, 946, DateTimeKind.Local).AddTicks(5306), false, null, null, "TransportationEmployee" },
                    { new Guid("3b1ccf79-0768-438c-8836-63d74559fe64"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 15, 12, 51, 9, 946, DateTimeKind.Local).AddTicks(5303), false, null, null, "Admin" },
                    { new Guid("5f676b28-fda8-462b-8e32-eed53dc07f01"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 15, 12, 51, 9, 946, DateTimeKind.Local).AddTicks(5301), false, null, null, "Customer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wallet_StoreId",
                table: "Wallet",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImage_ProductId",
                table: "ProductImage",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductInMenu_ProductInMenuId",
                table: "OrderDetails",
                column: "ProductInMenuId",
                principalTable: "ProductInMenu",
                principalColumn: "Id");
        }
    }
}
