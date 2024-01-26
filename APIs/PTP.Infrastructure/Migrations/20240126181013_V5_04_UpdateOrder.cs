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
                    { new Guid("8dac7a9a-b438-4149-b81a-5fdcce2819be"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 27, 1, 10, 12, 962, DateTimeKind.Local).AddTicks(6284), false, null, null, "Customer" },
                    { new Guid("90b3f5d2-f21a-4c25-8849-5616a969df07"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 27, 1, 10, 12, 962, DateTimeKind.Local).AddTicks(6289), false, null, null, "TransportationEmployee" },
                    { new Guid("aea060bb-7079-4764-a764-90e3782f6044"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 27, 1, 10, 12, 962, DateTimeKind.Local).AddTicks(6286), false, null, null, "Admin" },
                    { new Guid("eb4cf385-220b-4e2b-8c7f-11605259dcc4"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 27, 1, 10, 12, 962, DateTimeKind.Local).AddTicks(6260), false, null, null, "StoreManager" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("8dac7a9a-b438-4149-b81a-5fdcce2819be"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("90b3f5d2-f21a-4c25-8849-5616a969df07"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("aea060bb-7079-4764-a764-90e3782f6044"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("eb4cf385-220b-4e2b-8c7f-11605259dcc4"));

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
        }
    }
}
