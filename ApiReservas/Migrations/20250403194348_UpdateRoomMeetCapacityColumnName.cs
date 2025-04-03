using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiReservas.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomMeetCapacityColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Rooms",
                newName: "CapacityInHours");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CapacityInHours",
                table: "Rooms",
                newName: "Capacity");
        }
    }
}
