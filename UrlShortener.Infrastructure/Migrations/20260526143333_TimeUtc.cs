using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TimeUtc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Urls",
                newName: "CreatedAtUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAtUtc",
                table: "Urls",
                newName: "CreatedAt");
        }
    }
}
