using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpokaneChildren.Api.Migrations
{
    /// <inheritdoc />
    public partial class IndexDateTimeInAnnouncementAndEventTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Events_DateTime",
                table: "Events",
                column: "DateTime");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_DatePosted",
                table: "Announcements",
                column: "DatePosted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Events_DateTime",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_DatePosted",
                table: "Announcements");
        }
    }
}
