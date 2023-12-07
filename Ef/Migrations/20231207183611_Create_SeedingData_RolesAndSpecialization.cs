using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class Create_SeedingData_RolesAndSpecialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "65245382-8301-4da2-a5ac-6d66d34c9bbd", null, "Patient", "PATIENT" },
                    { "e6e6ce57-68c3-4c15-9c78-89a149407fc9", null, "Doctor", "DOCTOR" },
                    { "ee78c7c3-c557-42b6-b85d-fe90bd19a626", null, "Admin", "ADMIN" }
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
                keyValue: "65245382-8301-4da2-a5ac-6d66d34c9bbd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6e6ce57-68c3-4c15-9c78-89a149407fc9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee78c7c3-c557-42b6-b85d-fe90bd19a626");

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
