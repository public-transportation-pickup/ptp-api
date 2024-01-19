using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V5_03_AddCategoryStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductInMenu_ProductInMenuId",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductImage");

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

            migrationBuilder.RenameColumn(
                name: "ProductInMenuId",
                table: "OrderDetails",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductInMenuId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductId");

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
                    { new Guid("768b79e0-2246-4aaa-aa8d-e3684203e330"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 18, 10, 1, 45, 916, DateTimeKind.Local).AddTicks(235), false, null, null, "TransportationEmployee" },
                    { new Guid("997ff198-8adc-4553-ab8a-3b8b7870610c"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 18, 10, 1, 45, 916, DateTimeKind.Local).AddTicks(209), false, null, null, "StoreManager" },
                    { new Guid("cc0f6630-a3ef-4b9c-9867-f99c3e5ca57c"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 18, 10, 1, 45, 916, DateTimeKind.Local).AddTicks(233), false, null, null, "Admin" },
                    { new Guid("dbb3388c-38fa-4c89-8dd3-6c04343a3d65"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 18, 10, 1, 45, 916, DateTimeKind.Local).AddTicks(230), false, null, null, "Customer" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("768b79e0-2246-4aaa-aa8d-e3684203e330"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("997ff198-8adc-4553-ab8a-3b8b7870610c"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("cc0f6630-a3ef-4b9c-9867-f99c3e5ca57c"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("dbb3388c-38fa-4c89-8dd3-6c04343a3d65"));

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
