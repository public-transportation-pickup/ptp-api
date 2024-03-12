using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V1_02_Update_Order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("0402bdbe-8342-4cdf-a207-2be4f5a84b37"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2c59aea9-4733-4eba-bb49-62e2102e8943"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("334097d2-e8d0-42b0-894f-0621863f0ebc"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("7158ccbe-0543-4968-bc01-8c9add5a5fa5"));

            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "Order",
                type: "uniqueidentifier",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("1bd6e823-fcd5-488d-96e2-b47c90a71706"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 13, 1, 26, 37, 498, DateTimeKind.Local).AddTicks(9864), false, null, null, "Admin" },
                    { new Guid("5d91b00d-704c-4216-826e-497283c2649d"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 13, 1, 26, 37, 498, DateTimeKind.Local).AddTicks(9861), false, null, null, "Customer" },
                    { new Guid("ba132417-83b1-45ce-aaac-7295f39ee6c6"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 13, 1, 26, 37, 498, DateTimeKind.Local).AddTicks(9867), false, null, null, "TransportationEmployee" },
                    { new Guid("fa4a945b-8674-4b47-a1ad-4485bc427b37"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 13, 1, 26, 37, 498, DateTimeKind.Local).AddTicks(9842), false, null, null, "StoreManager" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("1bd6e823-fcd5-488d-96e2-b47c90a71706"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("5d91b00d-704c-4216-826e-497283c2649d"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("ba132417-83b1-45ce-aaac-7295f39ee6c6"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("fa4a945b-8674-4b47-a1ad-4485bc427b37"));

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Order");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("0402bdbe-8342-4cdf-a207-2be4f5a84b37"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 12, 16, 36, 31, 830, DateTimeKind.Local).AddTicks(7031), false, null, null, "StoreManager" },
                    { new Guid("2c59aea9-4733-4eba-bb49-62e2102e8943"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 12, 16, 36, 31, 830, DateTimeKind.Local).AddTicks(7070), false, null, null, "Admin" },
                    { new Guid("334097d2-e8d0-42b0-894f-0621863f0ebc"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 12, 16, 36, 31, 830, DateTimeKind.Local).AddTicks(7066), false, null, null, "Customer" },
                    { new Guid("7158ccbe-0543-4968-bc01-8c9add5a5fa5"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 12, 16, 36, 31, 830, DateTimeKind.Local).AddTicks(7072), false, null, null, "TransportationEmployee" }
                });
        }
    }
}
