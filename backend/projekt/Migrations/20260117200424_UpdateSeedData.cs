using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace projekt.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ChangePasswordRequired", "ContractType", "CreatedAt", "Email", "FirstName", "LastName", "LeaveBalance", "ManagerId", "MonthlyHours", "PasswordHash", "Position", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, false, "B2B", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@example.com", "Admin", "User", 26, null, 160, "password", "Administrator", "admin", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, false, "UoP", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "alice@example.com", "Manager", "Alice", 26, null, 160, "password", "Team Lead 1", "manager", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, false, "UoP", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "emily@example.com", "Manager", "Emily", 26, null, 160, "password", "Team Lead 2", "manager", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, false, "UoP", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "john@example.com", "John", "Doe", 20, 2, 160, "password", "Developer", "employee", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, false, "UoP", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane@example.com", "Jane", "Smith", 20, 3, 160, "password", "Designer", "employee", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, false, "UoP", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bob@example.com", "Bob", "Johnson", 20, 2, 160, "password", "QA Engineer", "employee", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "LeaveRequests",
                columns: new[] { "Id", "Comment", "CreatedAt", "EndDate", "LeaveType", "StartDate", "Status", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 1, "Urlop wypoczynkowy", new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Annual", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", new DateTime(2023, 11, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 2, "Zwolnienie lekarskie", new DateTime(2023, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sick", new DateTime(2023, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 3, "Urlop świąteczny", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Annual", new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", new DateTime(2023, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 6 },
                    { 4, "Urlop przed świętami", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Annual", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pending", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 5 },
                    { 5, "Choroba", new DateTime(2023, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sick", new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2023, 10, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 6, "Długi weekend", new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Annual", new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approved", new DateTime(2023, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "LeaveRequests",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
