using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V5_02_Update_Menu_Start_End : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("21e82a5c-4d04-4f27-b3fb-f88005283b43"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("5ccc72a1-39e4-4d17-a6c0-01424df7b8d6"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("8ab68ae6-4bfa-4b66-9b84-e1e832a214b1"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("8bfdca7e-993e-44db-aeeb-2a5b2bdd08fb"));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Menu",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Menu",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "EndTime",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Menu");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("21e82a5c-4d04-4f27-b3fb-f88005283b43"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 15, 12, 50, 16, 794, DateTimeKind.Local).AddTicks(3774), false, null, null, "StoreManager" },
                    { new Guid("5ccc72a1-39e4-4d17-a6c0-01424df7b8d6"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 15, 12, 50, 16, 794, DateTimeKind.Local).AddTicks(3795), false, null, null, "Customer" },
                    { new Guid("8ab68ae6-4bfa-4b66-9b84-e1e832a214b1"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 15, 12, 50, 16, 794, DateTimeKind.Local).AddTicks(3813), false, null, null, "TransportationEmployee" },
                    { new Guid("8bfdca7e-993e-44db-aeeb-2a5b2bdd08fb"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 15, 12, 50, 16, 794, DateTimeKind.Local).AddTicks(3810), false, null, null, "Admin" }
                });
        }
    }
}
