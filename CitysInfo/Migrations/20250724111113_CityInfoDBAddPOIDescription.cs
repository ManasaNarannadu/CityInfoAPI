using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CitysInfo.Migrations
{
    /// <inheritdoc />
    public partial class CityInfoDBAddPOIDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discription",
                table: "PointsOfInterest",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discription",
                table: "PointsOfInterest");
        }
    }
}
