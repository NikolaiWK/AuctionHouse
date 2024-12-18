namespace AuctionHouse.BidService.Rabbit;

public interface IBidPublisherService
{
    void PublishMessage(string bidEvent);
}