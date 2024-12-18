namespace AuctionHouse.AuctionManagementService.Rabbit.RabbitDtos;

public class AuctionEndedEvent : AuctionBaseEvent
{
    public DateTimeOffset EndTime { get; set; }
}