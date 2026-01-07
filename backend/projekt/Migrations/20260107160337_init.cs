using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace projekt.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysCount",
                table: "LeaveRequests");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "LeaveRequests",
                newName: "LeaveType");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "LeaveRequests",
                newName: "Comment");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LeaveType",
                table: "LeaveRequests",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Comment",
                table: "LeaveRequests",
                newName: "Reason");

            migrationBuilder.AddColumn<int>(
                name: "DaysCount",
                table: "LeaveRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
