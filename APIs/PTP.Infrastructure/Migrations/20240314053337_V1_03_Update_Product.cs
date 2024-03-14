using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V1_03_Update_Product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("127c6bbd-7222-47d2-b450-c567265c0076"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("92215943-281d-455f-a378-16ae4c0b633d"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("e5d3f0fb-6860-4a44-bded-02408a4bcc91"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f2d9a408-c49b-4da1-9c48-0e3e3ce6a203"));

            migrationBuilder.AddColumn<int>(
                name: "NumProcessParallel",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PreparationTime",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("58abe321-f0d7-4562-97bd-c2e27b0ba7e3"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 12, 33, 37, 556, DateTimeKind.Local).AddTicks(8411), false, null, null, "TransportationEmployee" },
                    { new Guid("858da6e2-dff9-47c4-8525-dae7bbb93f6d"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 12, 33, 37, 556, DateTimeKind.Local).AddTicks(8406), false, null, null, "Customer" },
                    { new Guid("b8744cbe-67d0-4f12-a31e-1b67a6de4db6"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 12, 33, 37, 556, DateTimeKind.Local).AddTicks(8409), false, null, null, "Admin" },
                    { new Guid("d0f551e7-fb59-48f5-8d37-06a597d214e4"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 12, 33, 37, 556, DateTimeKind.Local).AddTicks(8374), false, null, null, "StoreManager" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("58abe321-f0d7-4562-97bd-c2e27b0ba7e3"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("858da6e2-dff9-47c4-8525-dae7bbb93f6d"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b8744cbe-67d0-4f12-a31e-1b67a6de4db6"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("d0f551e7-fb59-48f5-8d37-06a597d214e4"));

            migrationBuilder.DropColumn(
                name: "NumProcessParallel",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "PreparationTime",
                table: "Product");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("127c6bbd-7222-47d2-b450-c567265c0076"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 7, 37, 9, 73, DateTimeKind.Local).AddTicks(8705), false, null, null, "TransportationEmployee" },
                    { new Guid("92215943-281d-455f-a378-16ae4c0b633d"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 7, 37, 9, 73, DateTimeKind.Local).AddTicks(8677), false, null, null, "StoreManager" },
                    { new Guid("e5d3f0fb-6860-4a44-bded-02408a4bcc91"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 7, 37, 9, 73, DateTimeKind.Local).AddTicks(8703), false, null, null, "Admin" },
                    { new Guid("f2d9a408-c49b-4da1-9c48-0e3e3ce6a203"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 7, 37, 9, 73, DateTimeKind.Local).AddTicks(8701), false, null, null, "Customer" }
                });
        }
    }
}
