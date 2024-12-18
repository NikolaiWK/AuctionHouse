using AuctionHouse.BidService.Service.RabbitDtos;
using AuctionHouse.BidService.Service.Repositories;

namespace AuctionHouse.BidService.Service.Services;

public interface IEventService
{
    public void ConsumeEvent(AuctionBaseEvent auctionEvent);
}

public class EventService(IAvailableAuctionsRepository availableAuctionsRepository) : IEventService
{
    public void ConsumeEvent(AuctionBaseEvent auctionEvent)
    {
        if (auctionEvent.AuctionType == AuctionType.AuctionStarted)
        {
            availableAuctionsRepository.StartAuction(auctionEvent.AuctionId);
        }
        else
        {
            availableAuctionsRepository.EndAuction(auctionEvent.AuctionId);
        }
    }
}