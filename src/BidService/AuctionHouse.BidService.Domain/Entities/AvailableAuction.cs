namespace AuctionHouse.BidService.Domain.Entities;

public class AvailableAuction
{
    public Guid AuctionId { get; set; }
    public AuctionType AuctionType { get; set; }
}

public enum AuctionType
{
    AuctionStarted = 0,
    AuctionFinished = 1,
}