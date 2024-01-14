using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V4_02_DeleteStoreType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_StoreType_StoreTypeId",
                table: "Store");

            migrationBuilder.DropTable(
                name: "StoreType");

            migrationBuilder.DropIndex(
                name: "IX_Store_StoreTypeId",
                table: "Store");

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("881f59b5-b092-4ef8-aa2f-9865ed62a667"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("af8b60eb-c51a-48c4-9c0d-ab65adfa2e0f"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c2803de2-17c0-41dd-ae71-74798c85b136"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("c4d66e4c-f73c-478d-b6fc-8fbcb35ea848"));

            migrationBuilder.DropColumn(
                name: "StoreTypeId",
                table: "Store");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ClosedTime",
                table: "Store",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "OpenedTime",
                table: "Store",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("1f1d3a6c-bf8a-417b-9135-5a9261695ca8"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 19, 13, 311, DateTimeKind.Local).AddTicks(882), false, null, null, "Customer" },
                    { new Guid("d8169739-7310-47cb-a841-3a11fffb50c1"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 19, 13, 311, DateTimeKind.Local).AddTicks(888), false, null, null, "TransportationEmployee" },
                    { new Guid("f04b304e-e863-4afc-b57d-ae63f0e475db"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 19, 13, 311, DateTimeKind.Local).AddTicks(885), false, null, null, "Admin" },
                    { new Guid("faceb5a1-74ff-47e3-b924-8a46a9fba059"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 19, 13, 311, DateTimeKind.Local).AddTicks(852), false, null, null, "StoreManager" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("1f1d3a6c-bf8a-417b-9135-5a9261695ca8"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("d8169739-7310-47cb-a841-3a11fffb50c1"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("f04b304e-e863-4afc-b57d-ae63f0e475db"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("faceb5a1-74ff-47e3-b924-8a46a9fba059"));

            migrationBuilder.DropColumn(
                name: "ClosedTime",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "OpenedTime",
                table: "Store");

            migrationBuilder.AddColumn<Guid>(
                name: "StoreTypeId",
                table: "Store",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StoreType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    { new Guid("881f59b5-b092-4ef8-aa2f-9865ed62a667"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 17, 9, 570, DateTimeKind.Local).AddTicks(8744), false, null, null, "TransportationEmployee" },
                    { new Guid("af8b60eb-c51a-48c4-9c0d-ab65adfa2e0f"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 17, 9, 570, DateTimeKind.Local).AddTicks(8716), false, null, null, "StoreManager" },
                    { new Guid("c2803de2-17c0-41dd-ae71-74798c85b136"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 17, 9, 570, DateTimeKind.Local).AddTicks(8741), false, null, null, "Admin" },
                    { new Guid("c4d66e4c-f73c-478d-b6fc-8fbcb35ea848"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 1, 14, 23, 17, 9, 570, DateTimeKind.Local).AddTicks(8738), false, null, null, "Customer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Store_StoreTypeId",
                table: "Store",
                column: "StoreTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_StoreType_StoreTypeId",
                table: "Store",
                column: "StoreTypeId",
                principalTable: "StoreType",
                principalColumn: "Id");
        }
    }
}
