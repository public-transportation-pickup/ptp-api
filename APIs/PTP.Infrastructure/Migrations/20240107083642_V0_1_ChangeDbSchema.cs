using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V0_1_ChangeDbSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeTables_RouteVarId",
                table: "TimeTables");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("7103171b-18bc-46c6-8f96-f4919c089160"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("752741b7-2677-400e-9f86-80a1a29c1349"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("d2573e3a-3488-4d7e-90cf-b644b06e60bb"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("ddf1d991-5ead-4dfa-848a-6580ba77d859"));

            migrationBuilder.DropColumn(
                name: "TimeTableId",
                table: "RouteVars");

            migrationBuilder.AddColumn<Guid>(
                name: "RouteId",
                table: "TimeTables",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "Index",
                table: "RouteStation",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("49c72f35-4d4d-4c81-be7d-32e55d69b597"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 15, 36, 42, 323, DateTimeKind.Local).AddTicks(9184), false, null, null, "Customer" },
                    { new Guid("745053f7-7af5-477a-987a-0083e82109e6"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 15, 36, 42, 323, DateTimeKind.Local).AddTicks(9187), false, null, null, "Admin" },
                    { new Guid("9a827a7f-3dc8-46ef-9c28-7c1d2046cb41"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 15, 36, 42, 323, DateTimeKind.Local).AddTicks(9165), false, null, null, "StoreManager" },
                    { new Guid("a9de9321-9dad-43c1-aa08-758e67f06a4c"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 15, 36, 42, 323, DateTimeKind.Local).AddTicks(9191), false, null, null, "TransportationEmployee" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_RouteId",
                table: "TimeTables",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_RouteVarId",
                table: "TimeTables",
                column: "RouteVarId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeTables_Route_RouteId",
                table: "TimeTables",
                column: "RouteId",
                principalTable: "Route",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeTables_Route_RouteId",
                table: "TimeTables");

            migrationBuilder.DropIndex(
                name: "IX_TimeTables_RouteId",
                table: "TimeTables");

            migrationBuilder.DropIndex(
                name: "IX_TimeTables_RouteVarId",
                table: "TimeTables");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("49c72f35-4d4d-4c81-be7d-32e55d69b597"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("745053f7-7af5-477a-987a-0083e82109e6"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("9a827a7f-3dc8-46ef-9c28-7c1d2046cb41"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("a9de9321-9dad-43c1-aa08-758e67f06a4c"));

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "TimeTables");

            migrationBuilder.AddColumn<Guid>(
                name: "TimeTableId",
                table: "RouteVars",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<byte>(
                name: "Index",
                table: "RouteStation",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("7103171b-18bc-46c6-8f96-f4919c089160"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 0, 40, 59, 198, DateTimeKind.Local).AddTicks(9975), false, null, null, "Admin" },
                    { new Guid("752741b7-2677-400e-9f86-80a1a29c1349"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 0, 40, 59, 198, DateTimeKind.Local).AddTicks(9977), false, null, null, "TransportationEmployee" },
                    { new Guid("d2573e3a-3488-4d7e-90cf-b644b06e60bb"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 0, 40, 59, 198, DateTimeKind.Local).AddTicks(9972), false, null, null, "Customer" },
                    { new Guid("ddf1d991-5ead-4dfa-848a-6580ba77d859"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 0, 40, 59, 198, DateTimeKind.Local).AddTicks(9955), false, null, null, "StoreManager" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_RouteVarId",
                table: "TimeTables",
                column: "RouteVarId",
                unique: true);
        }
    }
}
