using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PTP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VX_AddJWTToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("58abe321-f0d7-4562-97bd-c2e27b0ba7e3"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("858da6e2-dff9-47c4-8525-dae7bbb93f6d"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("b8744cbe-67d0-4f12-a31e-1b67a6de4db6"));

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: new Guid("d0f551e7-fb59-48f5-8d37-06a597d214e4"));

            migrationBuilder.AddColumn<string>(
                name: "JWTToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "JWTToken",
                table: "User");

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "IsDeleted", "ModificatedBy", "ModificationDate", "Name" },
                values: new object[,]
                {
                    { new Guid("58abe321-f0d7-4562-97bd-c2e27b0ba7e3"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 12, 33, 37, 556, DateTimeKind.Local).AddTicks(8411), false, null, null, "TransportationEmployee" },
                    { new Guid("858da6e2-dff9-47c4-8525-dae7bbb93f6d"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 12, 33, 37, 556, DateTimeKind.Local).AddTicks(8406), false, null, null, "Customer" },
                    { new Guid("b8744cbe-67d0-4f12-a31e-1b67a6de4db6"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 12, 33, 37, 556, DateTimeKind.Local).AddTicks(8409), false, null, null, "Admin" },
                    { new Guid("d0f551e7-fb59-48f5-8d37-06a597d214e4"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2024, 3, 14, 12, 33, 37, 556, DateTimeKind.Local).AddTicks(8374), false, null, null, "StoreManager" }
                });
        }
    }
}
