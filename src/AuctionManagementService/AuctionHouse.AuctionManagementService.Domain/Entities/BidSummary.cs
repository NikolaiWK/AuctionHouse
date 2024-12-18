namespace AuctionHouse.AuctionManagementService.Domain.Entities;

public class BidSummary
{
    public Guid BidSummaryId { get; set; }
    public Guid AuctionId { get; set; }
    public decimal CurrentHighestBid { get; set; }
    public int TotalBids { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }


    public Auction Auction { get; set; }
}