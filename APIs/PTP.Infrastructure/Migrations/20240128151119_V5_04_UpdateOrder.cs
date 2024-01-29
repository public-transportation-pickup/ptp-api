using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V5_04_UpdateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails");

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
                name: "StoreCode",
                table: "Store");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Payment",
                newName: "PaymentType");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderDetails",
                newName: "ProductMenuId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductMenuId");

            migrationBuilder.AlterColumn<string>(
                name: "OpenedTime",
                table: "Store",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<string>(
                name: "ClosedTime",
                table: "Store",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Payment",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CanceledReason",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PickUpTime",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "StartTime",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<string>(
                name: "EndTime",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("75f403de-f951-4f78-b27a-1025d7fbed15"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 28, 22, 11, 19, 319, DateTimeKind.Local).AddTicks(7614), false, null, null, "StoreManager" },
                    { new Guid("7ad0102a-6273-4e34-bd01-8cf2118d9e0c"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 28, 22, 11, 19, 319, DateTimeKind.Local).AddTicks(7638), false, null, null, "Admin" },
                    { new Guid("addc9612-1069-4fc0-a720-bed52d0d75ab"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 28, 22, 11, 19, 319, DateTimeKind.Local).AddTicks(7641), false, null, null, "TransportationEmployee" },
                    { new Guid("c9f382a7-c64d-40ad-a4ff-4a7fd23d3871"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 28, 22, 11, 19, 319, DateTimeKind.Local).AddTicks(7635), false, null, null, "Customer" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductInMenu_ProductMenuId",
                table: "OrderDetails",
                column: "ProductMenuId",
                principalTable: "ProductInMenu",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductInMenu_ProductMenuId",
                table: "OrderDetails");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("75f403de-f951-4f78-b27a-1025d7fbed15"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("7ad0102a-6273-4e34-bd01-8cf2118d9e0c"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("addc9612-1069-4fc0-a720-bed52d0d75ab"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c9f382a7-c64d-40ad-a4ff-4a7fd23d3871"));

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "CanceledReason",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PickUpTime",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "Payment",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ProductMenuId",
                table: "OrderDetails",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductMenuId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductId");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "OpenedTime",
                table: "Store",
                type: "time",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ClosedTime",
                table: "Store",
                type: "time",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "StoreCode",
                table: "Store",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "StartTime",
                table: "Menu",
                type: "time",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "EndTime",
                table: "Menu",
                type: "time",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }
    }
}
