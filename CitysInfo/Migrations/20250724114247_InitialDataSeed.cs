using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CitysInfo.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "The one with that big park. ", "New York City" },
                    { 2, "The one with the cathedral that was never really finished. ", "Antwerp" },
                    { 3, "the onw with that big tower. ", "Paris" }
                });

            migrationBuilder.InsertData(
                table: "PointsOfInterest",
                columns: new[] { "Id", "Discription", "Name", "cityId" },
                values: new object[,]
                {
                    { 1, "the most visited unbrun park in the united states.", "Central Park", 1 },
                    { 2, "A 102-story skycraper located in midtown manhattan.", "Empire State building", 1 },
                    { 3, "A gothic style cathdral, concevied by architects.", "cathdral", 2 },
                    { 4, "most beautiful and highest tower in world.", "efil tower", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PointsOfInterest",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
