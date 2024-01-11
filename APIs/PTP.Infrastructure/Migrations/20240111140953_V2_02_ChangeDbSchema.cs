using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2_02_ChangeDbSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("40c9768a-52d7-4e76-9a54-dc5d7dc92cab"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("9356a4e7-e61e-43a2-9819-52696abfd108"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("af717861-d8af-46e4-98cb-424ba41e465b"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f6537a66-ebec-4715-a368-2b19c0e43d7f"));

            migrationBuilder.AddColumn<double>(
                name: "AverageVelocity",
                table: "RouteVars",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Distance",
                table: "RouteVars",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "AverageVelocity",
                table: "Route",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("67091765-fd79-4493-b9d8-8f0c8c5c9470"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 21, 9, 53, 93, DateTimeKind.Local).AddTicks(2500), false, null, null, "TransportationEmployee" },
                    { new Guid("c2e09ab9-ec52-4497-b337-56261bc14014"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 21, 9, 53, 93, DateTimeKind.Local).AddTicks(2474), false, null, null, "StoreManager" },
                    { new Guid("c7d2bba4-0487-4f88-90b9-7966161d5528"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 21, 9, 53, 93, DateTimeKind.Local).AddTicks(2498), false, null, null, "Admin" },
                    { new Guid("cf2febc3-4a55-4d8e-b7b2-ee3a67883e8d"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 21, 9, 53, 93, DateTimeKind.Local).AddTicks(2494), false, null, null, "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("67091765-fd79-4493-b9d8-8f0c8c5c9470"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c2e09ab9-ec52-4497-b337-56261bc14014"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c7d2bba4-0487-4f88-90b9-7966161d5528"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("cf2febc3-4a55-4d8e-b7b2-ee3a67883e8d"));

            migrationBuilder.DropColumn(
                name: "AverageVelocity",
                table: "RouteVars");

            migrationBuilder.DropColumn(
                name: "Distance",
                table: "RouteVars");

            migrationBuilder.DropColumn(
                name: "AverageVelocity",
                table: "Route");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("40c9768a-52d7-4e76-9a54-dc5d7dc92cab"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 11, 47, 50, 241, DateTimeKind.Local).AddTicks(2254), false, null, null, "Admin" },
                    { new Guid("9356a4e7-e61e-43a2-9819-52696abfd108"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 11, 47, 50, 241, DateTimeKind.Local).AddTicks(2221), false, null, null, "StoreManager" },
                    { new Guid("af717861-d8af-46e4-98cb-424ba41e465b"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 11, 47, 50, 241, DateTimeKind.Local).AddTicks(2240), false, null, null, "Customer" },
                    { new Guid("f6537a66-ebec-4715-a368-2b19c0e43d7f"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 11, 47, 50, 241, DateTimeKind.Local).AddTicks(2256), false, null, null, "TransportationEmployee" }
                });
        }
    }
}
