using AuctionHouse.AuctionManagementService.Rabbit.RabbitDtos;

namespace AuctionHouse.AuctionManagementService.Rabbit;

public interface IAuctionPublisherService
{
    void PublishMessage(string auctionEvent);
}