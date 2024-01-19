using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V5_01_Change_Menu_Enity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("1f1d3a6c-bf8a-417b-9135-5a9261695ca8"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("d8169739-7310-47cb-a841-3a11fffb50c1"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f04b304e-e863-4afc-b57d-ae63f0e475db"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("faceb5a1-74ff-47e3-b924-8a46a9fba059"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "EndTime",
                table: "Menu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartTime",
                table: "Menu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("1f1d3a6c-bf8a-417b-9135-5a9261695ca8"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 19, 13, 311, DateTimeKind.Local).AddTicks(882), false, null, null, "Customer" },
                    { new Guid("d8169739-7310-47cb-a841-3a11fffb50c1"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 19, 13, 311, DateTimeKind.Local).AddTicks(888), false, null, null, "TransportationEmployee" },
                    { new Guid("f04b304e-e863-4afc-b57d-ae63f0e475db"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 19, 13, 311, DateTimeKind.Local).AddTicks(885), false, null, null, "Admin" },
                    { new Guid("faceb5a1-74ff-47e3-b924-8a46a9fba059"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 19, 13, 311, DateTimeKind.Local).AddTicks(852), false, null, null, "StoreManager" }
                });
        }
    }
}
