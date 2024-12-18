namespace AuctionHouse.AuctionManagementService.Domain.Entities;

public class Auction
{
    public Guid AuctionId { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal StartingPrice { get; set; }
    public AuctionStatus Status { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public BidSummary BidSummary { get; set; }
}

public enum AuctionStatus
{
    Created = 0,
    Started = 1,
    Ended = 2,
}