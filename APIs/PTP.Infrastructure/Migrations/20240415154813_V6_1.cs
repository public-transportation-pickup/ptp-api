using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V6_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("02cd17aa-c955-44ae-921c-8f6a4af5761d"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("13e4c4db-8e74-41d9-b499-e748841305b1"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("181a36ea-d8d0-4095-bd98-46737b36b83c"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("92b59f42-64ca-4fd4-b008-f327a61d159e"));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WalletLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionNo",
                table: "WalletLog",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("0d357a4d-e8bc-407d-855e-5d16bc9fdf52"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 4, 15, 22, 48, 12, 915, DateTimeKind.Local).AddTicks(642), false, null, null, "TransportationEmployee" },
                    { new Guid("4f752f8a-b084-497c-a4af-719255095066"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 4, 15, 22, 48, 12, 915, DateTimeKind.Local).AddTicks(621), false, null, null, "StoreManager" },
                    { new Guid("87f2a8af-fcaf-45c7-a1d0-a511d5184279"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 4, 15, 22, 48, 12, 915, DateTimeKind.Local).AddTicks(638), false, null, null, "Customer" },
                    { new Guid("9e11d9c0-8c98-4c77-9478-367a1384b2dd"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 4, 15, 22, 48, 12, 915, DateTimeKind.Local).AddTicks(640), false, null, null, "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("0d357a4d-e8bc-407d-855e-5d16bc9fdf52"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("4f752f8a-b084-497c-a4af-719255095066"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("87f2a8af-fcaf-45c7-a1d0-a511d5184279"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("9e11d9c0-8c98-4c77-9478-367a1384b2dd"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WalletLog");

            migrationBuilder.DropColumn(
                name: "TransactionNo",
                table: "WalletLog");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("02cd17aa-c955-44ae-921c-8f6a4af5761d"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 4, 15, 15, 51, 8, 773, DateTimeKind.Local).AddTicks(5847), false, null, null, "Customer" },
                    { new Guid("13e4c4db-8e74-41d9-b499-e748841305b1"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 4, 15, 15, 51, 8, 773, DateTimeKind.Local).AddTicks(5853), false, null, null, "Admin" },
                    { new Guid("181a36ea-d8d0-4095-bd98-46737b36b83c"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 4, 15, 15, 51, 8, 773, DateTimeKind.Local).AddTicks(5856), false, null, null, "TransportationEmployee" },
                    { new Guid("92b59f42-64ca-4fd4-b008-f327a61d159e"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 4, 15, 15, 51, 8, 773, DateTimeKind.Local).AddTicks(5775), false, null, null, "StoreManager" }
                });
        }
    }
}
