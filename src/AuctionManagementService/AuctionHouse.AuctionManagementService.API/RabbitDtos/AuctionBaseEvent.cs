namespace AuctionHouse.AuctionManagementService.API.RabbitDtos;

public class BidBaseEvent
{
    public Guid BidId { get; set; }
    public Guid AuctionId { get; set; }
    public int UserId { get; set; }
    public decimal BidAmount { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}