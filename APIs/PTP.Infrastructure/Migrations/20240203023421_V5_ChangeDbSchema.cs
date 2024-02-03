using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V5_ChangeDbSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("75f403de-f951-4f78-b27a-1025d7fbed15"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("7ad0102a-6273-4e34-bd01-8cf2118d9e0c"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("addc9612-1069-4fc0-a720-bed52d0d75ab"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c9f382a7-c64d-40ad-a4ff-4a7fd23d3871"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDistance",
                table: "RouteVars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PickUpTime",
                table: "Order",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("056254a6-d8c3-4969-a861-76de73a80437"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 3, 9, 34, 21, 690, DateTimeKind.Local).AddTicks(7758), false, null, null, "TransportationEmployee" },
                    { new Guid("1e07412e-24e1-4bdd-aeaa-bdfb0582b5fb"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 3, 9, 34, 21, 690, DateTimeKind.Local).AddTicks(7734), false, null, null, "StoreManager" },
                    { new Guid("46b1ad23-c95f-4480-8e22-f02627ed2fda"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 3, 9, 34, 21, 690, DateTimeKind.Local).AddTicks(7755), false, null, null, "Admin" },
                    { new Guid("a70559a7-21cc-4304-8715-e33f48f3d689"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 2, 3, 9, 34, 21, 690, DateTimeKind.Local).AddTicks(7753), false, null, null, "Customer" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("056254a6-d8c3-4969-a861-76de73a80437"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("1e07412e-24e1-4bdd-aeaa-bdfb0582b5fb"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("46b1ad23-c95f-4480-8e22-f02627ed2fda"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("a70559a7-21cc-4304-8715-e33f48f3d689"));

            migrationBuilder.DropColumn(
                name: "IsDistance",
                table: "RouteVars");

            migrationBuilder.AlterColumn<string>(
                name: "PickUpTime",
                table: "Order",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("75f403de-f951-4f78-b27a-1025d7fbed15"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 28, 22, 11, 19, 319, DateTimeKind.Local).AddTicks(7614), false, null, null, "StoreManager" },
                    { new Guid("7ad0102a-6273-4e34-bd01-8cf2118d9e0c"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 28, 22, 11, 19, 319, DateTimeKind.Local).AddTicks(7638), false, null, null, "Admin" },
                    { new Guid("addc9612-1069-4fc0-a720-bed52d0d75ab"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 28, 22, 11, 19, 319, DateTimeKind.Local).AddTicks(7641), false, null, null, "TransportationEmployee" },
                    { new Guid("c9f382a7-c64d-40ad-a4ff-4a7fd23d3871"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 28, 22, 11, 19, 319, DateTimeKind.Local).AddTicks(7635), false, null, null, "Customer" }
                });
        }
    }
}
