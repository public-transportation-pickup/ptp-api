using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2_ChangeDbschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("015d084e-8420-4c24-a171-d7faa136e5f7"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("14e18b04-fb1e-4a5e-a733-8a7858bc340e"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("84d50a44-bde5-4a83-ac94-ef84a88b3bd5"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f5b3dd1a-4815-447d-93ec-2089cd4fa3f2"));

            migrationBuilder.AddColumn<Guid>(
                name: "RouteVarId",
                table: "RouteStation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

            migrationBuilder.CreateIndex(
                name: "IX_RouteStation_RouteVarId",
                table: "RouteStation",
                column: "RouteVarId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteStation_RouteVars_RouteVarId",
                table: "RouteStation",
                column: "RouteVarId",
                principalTable: "RouteVars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteStation_RouteVars_RouteVarId",
                table: "RouteStation");

            migrationBuilder.DropIndex(
                name: "IX_RouteStation_RouteVarId",
                table: "RouteStation");

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

            migrationBuilder.DropColumn(
                name: "RouteVarId",
                table: "RouteStation");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("015d084e-8420-4c24-a171-d7faa136e5f7"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 0, 8, 12, 434, DateTimeKind.Local).AddTicks(8911), false, null, null, "Customer" },
                    { new Guid("14e18b04-fb1e-4a5e-a733-8a7858bc340e"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 0, 8, 12, 434, DateTimeKind.Local).AddTicks(8925), false, null, null, "Admin" },
                    { new Guid("84d50a44-bde5-4a83-ac94-ef84a88b3bd5"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 0, 8, 12, 434, DateTimeKind.Local).AddTicks(8927), false, null, null, "TransportationEmployee" },
                    { new Guid("f5b3dd1a-4815-447d-93ec-2089cd4fa3f2"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 11, 0, 8, 12, 434, DateTimeKind.Local).AddTicks(8892), false, null, null, "StoreManager" }
                });
        }
    }
}
