using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourAgencyApp.Migrations
{
    /// <inheritdoc />
    public partial class IsOnTour_NotMapped : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnTour",
                table: "Tours");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnTour",
                table: "Tours",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
