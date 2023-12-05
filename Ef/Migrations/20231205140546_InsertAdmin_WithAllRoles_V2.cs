using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class InsertAdmin_WithAllRoles_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4031e1bd-d06d-4b5b-887d-26ca7a1813a5", null, "Patient", "PATIENT" },
                    { "58d3d040-63af-456a-bac4-a22c52ed7113", null, "Doctor", "DOCTOR" },
                    { "af4f99d9-a1f9-466b-99d7-af4cf3d913ba", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateOfAdd", "DateOfBirth", "Discriminator", "Email", "EmailConfirmed", "Gender", "Image", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "bfbf7d4e-2d6f-4637-a025-5c91f72a3b9d", 0, "77116e74-bf42-42f7-8b20-2b51edb10499", new DateTime(2023, 12, 5, 16, 5, 45, 880, DateTimeKind.Local).AddTicks(4907), new DateTime(1988, 12, 5, 16, 5, 45, 880, DateTimeKind.Local).AddTicks(4841), "ApplicationUser", "Abdelaziz.2023@gmail.com", false, 1, null, false, null, null, null, "13S0b/QBead9nWGDtE3BnhD4UKIsfpDkCeTPDx1HpV8=", null, false, "1887525e-3d16-4cd8-a05d-483ae4c8c03d", false, "Abdelaziz" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "af4f99d9-a1f9-466b-99d7-af4cf3d913ba", "bfbf7d4e-2d6f-4637-a025-5c91f72a3b9d" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4031e1bd-d06d-4b5b-887d-26ca7a1813a5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58d3d040-63af-456a-bac4-a22c52ed7113");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "af4f99d9-a1f9-466b-99d7-af4cf3d913ba", "bfbf7d4e-2d6f-4637-a025-5c91f72a3b9d" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "af4f99d9-a1f9-466b-99d7-af4cf3d913ba");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "bfbf7d4e-2d6f-4637-a025-5c91f72a3b9d");
        }
    }
}
