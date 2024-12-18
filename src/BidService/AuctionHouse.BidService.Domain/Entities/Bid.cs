namespace AuctionHouse.BidService.Domain.Entities;

public class Bid
{
    public Guid BidId { get; set; }
    public Guid AuctionId { get; set; }
    public int UserId { get; set; }
    public decimal BidAmount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}