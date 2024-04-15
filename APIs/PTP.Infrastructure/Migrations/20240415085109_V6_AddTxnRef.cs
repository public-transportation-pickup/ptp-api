using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V6_AddTxnRef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("5b45d1a5-b434-4f2d-9070-578d0458b171"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("7f4dfffc-adaf-491e-b39e-019e854596d7"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("987179c7-d0d4-401e-aef1-007ec9735616"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("af6dd891-df8b-4271-839a-49d911b3fc14"));

            migrationBuilder.AddColumn<string>(
                name: "TxnRef",
                table: "WalletLog",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "TxnRef",
                table: "WalletLog");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("5b45d1a5-b434-4f2d-9070-578d0458b171"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 24, 4, 18, 49, 802, DateTimeKind.Local).AddTicks(1998), false, null, null, "Admin" },
                    { new Guid("7f4dfffc-adaf-491e-b39e-019e854596d7"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 24, 4, 18, 49, 802, DateTimeKind.Local).AddTicks(2000), false, null, null, "TransportationEmployee" },
                    { new Guid("987179c7-d0d4-401e-aef1-007ec9735616"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 24, 4, 18, 49, 802, DateTimeKind.Local).AddTicks(1995), false, null, null, "Customer" },
                    { new Guid("af6dd891-df8b-4271-839a-49d911b3fc14"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 24, 4, 18, 49, 802, DateTimeKind.Local).AddTicks(1976), false, null, null, "StoreManager" }
                });
        }
    }
}
