using AuctionHouse.AuctionManagementService.API.RabbitDtos;
using AuctionHouse.AuctionManagementService.API.Repositories;
using AuctionHouse.AuctionManagementService.Domain.Entities;

namespace AuctionHouse.AuctionManagementService.API.Services;

public interface IEventService
{
    public Task ConsumeEvent(BidBaseEvent auctionEvent);
}

public class EventService(IAuctionRepository auctionRepository, ILogger<EventService> logger) : IEventService
{
    public async Task ConsumeEvent(BidBaseEvent auctionEvent)
    {
        logger.LogInformation($"{auctionEvent} auction event received");
        var auction = await auctionRepository.GetAuction(auctionEvent.AuctionId);
        if (auction is { Status: AuctionStatus.Started })
        {
            logger.LogInformation($"Found existing auction with id: {auction.AuctionId}");
            if (auction.StartingPrice <= auctionEvent.BidAmount && auction.BidSummary.CurrentHighestBid <= auctionEvent.BidAmount)
            {
                logger.LogInformation($"Bid is larger than start price and current highest bid with value: {auctionEvent.BidAmount}");
                await auctionRepository.PlaceNewBid(auctionEvent.AuctionId, auctionEvent.UserId, auctionEvent.BidAmount);
                //Send some event to the notify service when created...
            }
            else
            {
                logger.LogInformation($"Bid is not larger than start price and current highest bid with value: {auctionEvent.BidAmount}");
            }
        }
    }
}