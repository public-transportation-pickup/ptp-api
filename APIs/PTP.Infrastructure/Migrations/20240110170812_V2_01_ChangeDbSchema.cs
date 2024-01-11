using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2_01_ChangeDbSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_StoreStation_StoreStationId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "StoreStation");

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

            migrationBuilder.RenameColumn(
                name: "StoreStationId",
                table: "Order",
                newName: "StationId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_StoreStationId",
                table: "Order",
                newName: "IX_Order_StationId");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreTypeId",
                table: "Store",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StoreId",
                table: "Station",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StoreType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreType", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Store_StoreTypeId",
                table: "Store",
                column: "StoreTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Station_StoreId",
                table: "Station",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Station_StationId",
                table: "Order",
                column: "StationId",
                principalTable: "Station",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Station_Store_StoreId",
                table: "Station",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_StoreType_StoreTypeId",
                table: "Store",
                column: "StoreTypeId",
                principalTable: "StoreType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Station_StationId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Station_Store_StoreId",
                table: "Station");

            migrationBuilder.DropForeignKey(
                name: "FK_Store_StoreType_StoreTypeId",
                table: "Store");

            migrationBuilder.DropTable(
                name: "StoreType");

            migrationBuilder.DropIndex(
                name: "IX_Store_StoreTypeId",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Station_StoreId",
                table: "Station");

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

            migrationBuilder.DropColumn(
                name: "StoreTypeId",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Station");

            migrationBuilder.RenameColumn(
                name: "StationId",
                table: "Order",
                newName: "StoreStationId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_StationId",
                table: "Order",
                newName: "IX_Order_StoreStationId");

            migrationBuilder.CreateTable(
                name: "StoreStation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Distance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreStation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreStation_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreStation_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_StoreStation_StationId",
                table: "StoreStation",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreStation_StoreId",
                table: "StoreStation",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_StoreStation_StoreStationId",
                table: "Order",
                column: "StoreStationId",
                principalTable: "StoreStation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
