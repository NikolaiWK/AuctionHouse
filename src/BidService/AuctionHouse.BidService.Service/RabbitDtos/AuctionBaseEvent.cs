namespace AuctionHouse.BidService.Service.RabbitDtos;

public class AuctionBaseEvent
{
    public AuctionType AuctionType { get; set; }
    public Guid AuctionId { get; set; }

}

public enum AuctionType
{
    AuctionStarted = 0,
    AuctionFinished = 1,
}