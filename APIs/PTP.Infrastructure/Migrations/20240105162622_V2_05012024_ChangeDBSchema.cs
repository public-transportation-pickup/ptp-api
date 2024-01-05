using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2_05012024_ChangeDBSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Trip_Vehicle_VehicleId",
                table: "Trip");

            migrationBuilder.DropTable(
                name: "Vehicle");

            migrationBuilder.DropIndex(
                name: "IX_Trip_VehicleId",
                table: "Trip");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("123bd1ea-e312-417a-9a3c-55c8cd37c9fc"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("5b21c834-ee03-496e-a326-fd2177f4b6c9"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("99fec35b-dd86-4123-a2e2-005ca2d6cf56"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c852e84f-c6a1-4a5f-9e4e-303337452b5a"));

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Trip");

            migrationBuilder.DropColumn(
                name: "UnitInStock",
                table: "ProductInMenu");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "OrderDetails",
                newName: "ProductInMenuId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductInMenuId");

            migrationBuilder.AddColumn<decimal>(
                name: "CommisionRate",
                table: "Store",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RouteStation",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "DistanceFromStart",
                table: "RouteStation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DistanceToNext",
                table: "RouteStation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DurationFromStart",
                table: "RouteStation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DurationToNext",
                table: "RouteStation",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "RouteType",
                table: "Route",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "HeadWay",
                table: "Route",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InBoundDescription",
                table: "Route",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InBoundName",
                table: "Route",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OperationTime",
                table: "Route",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OutBoundDescription",
                table: "Route",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OutBoundName",
                table: "Route",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RouteNumber",
                table: "Route",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeOfTrip",
                table: "Route",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalTrip",
                table: "Route",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ActualPrice",
                table: "ProductInMenu",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CommissionRate",
                table: "Order",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Schedules_Trip_TripId",
                        column: x => x.TripId,
                        principalTable: "Trip",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("0b94fbaa-a011-4b7e-b389-0d90e58e44bf"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 5, 23, 26, 22, 506, DateTimeKind.Local).AddTicks(6716), false, null, null, "Admin" },
                    { new Guid("400c2818-5838-4caf-b15d-55a30cfdf079"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 5, 23, 26, 22, 506, DateTimeKind.Local).AddTicks(6706), false, null, null, "Customer" },
                    { new Guid("8aa336a7-21e3-4a2a-8eec-26993f613977"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 5, 23, 26, 22, 506, DateTimeKind.Local).AddTicks(6692), false, null, null, "StoreManager" },
                    { new Guid("f45f59f7-7a12-410e-8149-d97fb2cbc92b"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 5, 23, 26, 22, 506, DateTimeKind.Local).AddTicks(6718), false, null, null, "TransportationEmployee" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_StationId",
                table: "Schedules",
                column: "StationId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_TripId",
                table: "Schedules",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_ProductInMenu_ProductInMenuId",
                table: "OrderDetails",
                column: "ProductInMenuId",
                principalTable: "ProductInMenu",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_ProductInMenu_ProductInMenuId",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("0b94fbaa-a011-4b7e-b389-0d90e58e44bf"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("400c2818-5838-4caf-b15d-55a30cfdf079"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("8aa336a7-21e3-4a2a-8eec-26993f613977"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f45f59f7-7a12-410e-8149-d97fb2cbc92b"));

            migrationBuilder.DropColumn(
                name: "CommisionRate",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RouteStation");

            migrationBuilder.DropColumn(
                name: "DistanceFromStart",
                table: "RouteStation");

            migrationBuilder.DropColumn(
                name: "DistanceToNext",
                table: "RouteStation");

            migrationBuilder.DropColumn(
                name: "DurationFromStart",
                table: "RouteStation");

            migrationBuilder.DropColumn(
                name: "DurationToNext",
                table: "RouteStation");

            migrationBuilder.DropColumn(
                name: "HeadWay",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "InBoundDescription",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "InBoundName",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "OperationTime",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "OutBoundDescription",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "OutBoundName",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "RouteNumber",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "TimeOfTrip",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "TotalTrip",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "ActualPrice",
                table: "ProductInMenu");

            migrationBuilder.DropColumn(
                name: "CommissionRate",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "ProductInMenuId",
                table: "OrderDetails",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_ProductInMenuId",
                table: "OrderDetails",
                newName: "IX_OrderDetails_ProductId");

            migrationBuilder.AddColumn<Guid>(
                name: "VehicleId",
                table: "Trip",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "RouteType",
                table: "Route",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UnitInStock",
                table: "ProductInMenu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ModificatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfSeats = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("123bd1ea-e312-417a-9a3c-55c8cd37c9fc"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2023, 12, 30, 18, 24, 33, 484, DateTimeKind.Local).AddTicks(3895), false, null, null, "TransportationEmployee" },
                    { new Guid("5b21c834-ee03-496e-a326-fd2177f4b6c9"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2023, 12, 30, 18, 24, 33, 484, DateTimeKind.Local).AddTicks(3875), false, null, null, "StoreManager" },
                    { new Guid("99fec35b-dd86-4123-a2e2-005ca2d6cf56"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2023, 12, 30, 18, 24, 33, 484, DateTimeKind.Local).AddTicks(3893), false, null, null, "Admin" },
                    { new Guid("c852e84f-c6a1-4a5f-9e4e-303337452b5a"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2023, 12, 30, 18, 24, 33, 484, DateTimeKind.Local).AddTicks(3890), false, null, null, "Customer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trip_VehicleId",
                table: "Trip",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Product_ProductId",
                table: "OrderDetails",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_Vehicle_VehicleId",
                table: "Trip",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
