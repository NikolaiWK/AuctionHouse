using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionHouse.AuctionManagementService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToBidSummary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "BidSummaries",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BidSummaries");
        }
    }
}
