namespace AuctionHouse.AuctionManagementService.Rabbit.RabbitDtos;

public class AuctionStartedEvent : AuctionBaseEvent
{
    public DateTimeOffset StartTime { get; set; }
}