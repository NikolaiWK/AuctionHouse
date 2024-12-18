using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionHouse.BidService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitBid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AvailableAuctions",
                columns: table => new
                {
                    AuctionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuctionType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailableAuctions", x => x.AuctionId);
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    BidId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuctionId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BidAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.BidId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvailableAuctions");

            migrationBuilder.DropTable(
                name: "Bids");
        }
    }
}
