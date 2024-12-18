using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionHouse.AuctionManagementService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AuctionInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    AuctionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    StartingPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.AuctionId);
                });

            migrationBuilder.CreateTable(
                name: "BidSummaries",
                columns: table => new
                {
                    BidSummaryId = table.Column<Guid>(type: "uuid", nullable: false),
                    AuctionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentHighestBid = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalBids = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidSummaries", x => x.BidSummaryId);
                    table.ForeignKey(
                        name: "FK_BidSummaries_Auctions_AuctionId",
                        column: x => x.AuctionId,
                        principalTable: "Auctions",
                        principalColumn: "AuctionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BidSummaries_AuctionId",
                table: "BidSummaries",
                column: "AuctionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BidSummaries");

            migrationBuilder.DropTable(
                name: "Auctions");
        }
    }
}
