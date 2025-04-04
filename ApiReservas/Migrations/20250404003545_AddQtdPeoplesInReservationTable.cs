using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiReservas.Migrations
{
    /// <inheritdoc />
    public partial class AddQtdPeoplesInReservationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QtdPeoples",
                table: "Reservations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QtdPeoples",
                table: "Reservations");
        }
    }
}
