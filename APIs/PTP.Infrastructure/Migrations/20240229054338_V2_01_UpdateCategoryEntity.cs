using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2_01_UpdateCategoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("5f0e282c-cff6-48b7-bca6-95ff40106590"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("870e33a9-e5de-40ca-91e3-3f234fa43338"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("a87a471d-3beb-4567-b31b-57ab734db62a"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f3bdb0cb-79fe-437b-8560-52f8d1b37227"));

            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Category",
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "ImageName",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Category");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("5f0e282c-cff6-48b7-bca6-95ff40106590"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 5, 16, 12, 5, 702, DateTimeKind.Local).AddTicks(2201), false, null, null, "Admin" },
                    { new Guid("870e33a9-e5de-40ca-91e3-3f234fa43338"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 5, 16, 12, 5, 702, DateTimeKind.Local).AddTicks(2170), false, null, null, "StoreManager" },
                    { new Guid("a87a471d-3beb-4567-b31b-57ab734db62a"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 5, 16, 12, 5, 702, DateTimeKind.Local).AddTicks(2189), false, null, null, "Customer" },
                    { new Guid("f3bdb0cb-79fe-437b-8560-52f8d1b37227"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 5, 16, 12, 5, 702, DateTimeKind.Local).AddTicks(2204), false, null, null, "TransportationEmployee" }
                });
        }
    }
}
