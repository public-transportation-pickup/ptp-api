using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V4_01_ChangeStoreEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "ClosedTime",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "CommisionRate",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "OpenedTime",
                table: "Store");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("881f59b5-b092-4ef8-aa2f-9865ed62a667"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 17, 9, 570, DateTimeKind.Local).AddTicks(8744), false, null, null, "TransportationEmployee" },
                    { new Guid("af8b60eb-c51a-48c4-9c0d-ab65adfa2e0f"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 17, 9, 570, DateTimeKind.Local).AddTicks(8716), false, null, null, "StoreManager" },
                    { new Guid("c2803de2-17c0-41dd-ae71-74798c85b136"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 17, 9, 570, DateTimeKind.Local).AddTicks(8741), false, null, null, "Admin" },
                    { new Guid("c4d66e4c-f73c-478d-b6fc-8fbcb35ea848"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 17, 9, 570, DateTimeKind.Local).AddTicks(8738), false, null, null, "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("881f59b5-b092-4ef8-aa2f-9865ed62a667"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("af8b60eb-c51a-48c4-9c0d-ab65adfa2e0f"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c2803de2-17c0-41dd-ae71-74798c85b136"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c4d66e4c-f73c-478d-b6fc-8fbcb35ea848"));

            migrationBuilder.AddColumn<byte>(
                name: "ClosedTime",
                table: "Store",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<decimal>(
                name: "CommisionRate",
                table: "Store",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<byte>(
                name: "OpenedTime",
                table: "Store",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

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
        }
    }
}
