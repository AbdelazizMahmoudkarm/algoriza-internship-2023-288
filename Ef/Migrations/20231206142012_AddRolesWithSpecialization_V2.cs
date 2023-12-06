using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesWithSpecialization_V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1743c055-aa90-4d6b-85d0-1a21085f56a6", null, "Doctor", "DOCTOR" },
                    { "6d0f44f2-9fe5-4dc9-ade3-db0f99079d03", null, "Patient", "PATIENT" },
                    { "7a9b126c-e62c-4483-9c48-46e938a0b148", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "Specializations",
                columns: new[] { "Id", "ArName", "Name" },
                values: new object[,]
                {
                    { 1, "عيون", "Eyes" },
                    { 2, "جلدية", "Skin" },
                    { 3, "عظام", "Bones" },
                    { 4, "اسنان", "Teeth" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1743c055-aa90-4d6b-85d0-1a21085f56a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d0f44f2-9fe5-4dc9-ade3-db0f99079d03");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a9b126c-e62c-4483-9c48-46e938a0b148");

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Specializations",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
