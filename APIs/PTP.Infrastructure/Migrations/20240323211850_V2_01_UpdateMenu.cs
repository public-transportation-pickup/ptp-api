using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V2_01_UpdateMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("0f20a48a-6988-49d2-b339-d864f91c0c45"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("3782e3b9-331d-4002-94dc-9359482b8eb4"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("53fb87d8-52b6-411a-aae6-a9dab02b31a1"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("9088829a-4c9a-4588-8717-b555ef921635"));

            migrationBuilder.DropColumn(
                name: "MenuId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "MaxNumOrderProcess",
                table: "Menu");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnAmount",
                table: "Order",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DateApply",
                table: "Menu",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Menu",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApplyForAll",
                table: "Menu",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Menu",
                type: "datetime2",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ReturnAmount",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "IsApplyForAll",
                table: "Menu");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Menu");

            migrationBuilder.AddColumn<Guid>(
                name: "MenuId",
                table: "Order",
                type: "uniqueidentifier",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateApply",
                table: "Menu",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "MaxNumOrderProcess",
                table: "Menu",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("0f20a48a-6988-49d2-b339-d864f91c0c45"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 19, 22, 2, 16, 561, DateTimeKind.Local).AddTicks(552), false, null, null, "StoreManager" },
                    { new Guid("3782e3b9-331d-4002-94dc-9359482b8eb4"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 19, 22, 2, 16, 561, DateTimeKind.Local).AddTicks(584), false, null, null, "Admin" },
                    { new Guid("53fb87d8-52b6-411a-aae6-a9dab02b31a1"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 19, 22, 2, 16, 561, DateTimeKind.Local).AddTicks(587), false, null, null, "TransportationEmployee" },
                    { new Guid("9088829a-4c9a-4588-8717-b555ef921635"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 19, 22, 2, 16, 561, DateTimeKind.Local).AddTicks(582), false, null, null, "Customer" }
                });
        }
    }
}
