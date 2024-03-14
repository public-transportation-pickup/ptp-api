using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V1_02_Update_Menu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("225a2fca-e19c-4e69-b977-cbb785690932"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3537630f-9b48-48dc-b20a-ad173ecf3e9a"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("461ad301-9d95-421c-b910-5b4b48123631"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("cc1384ac-bf74-429d-a942-9bbda707ea0a"));

            migrationBuilder.DropColumn(
                name: "NumOrderEstimated",
                table: "Menu");

            migrationBuilder.RenameColumn(
                name: "NumOrderSold",
                table: "Menu",
                newName: "MaxNumOrderProcess");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "MaxNumOrderProcess",
                table: "Menu",
                newName: "NumOrderSold");

            migrationBuilder.AddColumn<int>(
                name: "NumOrderEstimated",
                table: "Menu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("225a2fca-e19c-4e69-b977-cbb785690932"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 1, 38, 24, 946, DateTimeKind.Local).AddTicks(4423), false, null, null, "Admin" },
                    { new Guid("3537630f-9b48-48dc-b20a-ad173ecf3e9a"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 1, 38, 24, 946, DateTimeKind.Local).AddTicks(4420), false, null, null, "Customer" },
                    { new Guid("461ad301-9d95-421c-b910-5b4b48123631"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 1, 38, 24, 946, DateTimeKind.Local).AddTicks(4426), false, null, null, "TransportationEmployee" },
                    { new Guid("cc1384ac-bf74-429d-a942-9bbda707ea0a"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 1, 38, 24, 946, DateTimeKind.Local).AddTicks(4397), false, null, null, "StoreManager" }
                });
        }
    }
}
