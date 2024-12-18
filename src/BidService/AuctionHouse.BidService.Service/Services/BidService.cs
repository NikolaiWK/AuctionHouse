using System.Text.Json;
using AuctionHouse.BidService.Domain.Entities;
using AuctionHouse.BidService.Rabbit;
using AuctionHouse.BidService.Rabbit.RabbitDtos;
using AuctionHouse.BidService.Service.Repositories;

namespace AuctionHouse.BidService.Service.Services;

public interface IBidService
{
    public Task<Guid?> PlaceBid(Guid auctionId, decimal amount, int userId);
}

public class BidService(IAvailableAuctionsRepository availableAuctionsRepository, IBidRepository bidRepository, IBidPublisherService publisherService) : IBidService
{
    public async Task<Guid?> PlaceBid(Guid auctionId, decimal amount, int userId)
    {
        var auctionAvailable = await availableAuctionsRepository.GetAuctionState(auctionId);

        if (auctionAvailable is AuctionType.AuctionStarted)
        {
            var bidId = Guid.NewGuid();
            var bid = new Bid
            {
                BidId = bidId,
                AuctionId = auctionId,
                UserId = userId,
                BidAmount = amount,
                CreatedAt = DateTimeOffset.UtcNow
            };

            bidRepository.CreateBid(bid);

            var serializedEvent = JsonSerializer.Serialize(new BidBaseEvent
            {
                AuctionId = bid.AuctionId,
                UserId = bid.UserId,
                BidAmount = bid.BidAmount,
                CreatedAt = bid.CreatedAt,
                BidId = bid.BidId
            });

            publisherService.PublishMessage(serializedEvent);
            return bidId;
        }

        return null;
    }
}