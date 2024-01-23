using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V5_01_Update_Order : Migration
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

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Payment",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("36509517-1977-4456-ac42-091adbd3c562"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 24, 0, 29, 8, 432, DateTimeKind.Local).AddTicks(845), false, null, null, "Customer" },
                    { new Guid("3c743c1d-179e-4d84-9939-6ee95bfd4680"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 24, 0, 29, 8, 432, DateTimeKind.Local).AddTicks(824), false, null, null, "StoreManager" },
                    { new Guid("4f1a2ae6-f1df-4f9d-a3b4-8720e6a1a54f"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 24, 0, 29, 8, 432, DateTimeKind.Local).AddTicks(849), false, null, null, "Admin" },
                    { new Guid("822bca68-4bf5-4a9b-ac03-dc91e67ea78d"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 24, 0, 29, 8, 432, DateTimeKind.Local).AddTicks(852), false, null, null, "TransportationEmployee" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("36509517-1977-4456-ac42-091adbd3c562"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3c743c1d-179e-4d84-9939-6ee95bfd4680"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("4f1a2ae6-f1df-4f9d-a3b4-8720e6a1a54f"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("822bca68-4bf5-4a9b-ac03-dc91e67ea78d"));

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "Payment",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "StoreCode",
                table: "Store",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
