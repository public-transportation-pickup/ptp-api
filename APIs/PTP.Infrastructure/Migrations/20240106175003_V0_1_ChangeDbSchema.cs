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

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("17adec58-de65-4511-b0d5-b2aa33b25bfb"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 0, 50, 3, 295, DateTimeKind.Local).AddTicks(3509), false, null, null, "Customer" },
                    { new Guid("2962c665-5d8e-43e6-bb87-471aee9de49e"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 0, 50, 3, 295, DateTimeKind.Local).AddTicks(3493), false, null, null, "StoreManager" },
                    { new Guid("f099ff49-4b0b-4884-b2ef-2cfa515b0089"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 0, 50, 3, 295, DateTimeKind.Local).AddTicks(3514), false, null, null, "TransportationEmployee" },
                    { new Guid("f477764c-1f64-4ff6-bed3-e4259becda76"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 7, 0, 50, 3, 295, DateTimeKind.Local).AddTicks(3512), false, null, null, "Admin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeTables_RouteVarId",
                table: "TimeTables",
                column: "RouteVarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TimeTables_RouteVarId",
                table: "TimeTables");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("17adec58-de65-4511-b0d5-b2aa33b25bfb"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("2962c665-5d8e-43e6-bb87-471aee9de49e"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f099ff49-4b0b-4884-b2ef-2cfa515b0089"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f477764c-1f64-4ff6-bed3-e4259becda76"));

            migrationBuilder.AddColumn<Guid>(
                name: "TimeTableId",
                table: "RouteVars",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
