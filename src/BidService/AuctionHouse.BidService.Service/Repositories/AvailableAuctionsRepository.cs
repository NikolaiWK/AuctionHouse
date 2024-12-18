using AuctionHouse.BidService.Domain.Entities;
using AuctionHouse.BidService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.BidService.Service.Repositories;

public interface IAvailableAuctionsRepository
{
    void StartAuction(Guid auctionId);
    void EndAuction(Guid auctionId);
    Task<AuctionType?> GetAuctionState(Guid auctionId);
}

public class AvailableAuctionsRepository(BidDbContext context) : IAvailableAuctionsRepository
{
    public void StartAuction(Guid auctionId)
    {
        var existing = context.Set<AvailableAuction>().FirstOrDefault(x => x.AuctionId == auctionId);
        if (existing == null)
        {
            context.Set<AvailableAuction>().Add(new AvailableAuction
            {
                AuctionId = auctionId,
                AuctionType = AuctionType.AuctionStarted
            });

            context.SaveChanges();
        }
    }

    public void EndAuction(Guid auctionId)
    {
        var existing = context.Set<AvailableAuction>().FirstOrDefault(x => x.AuctionId == auctionId);
        if (existing != null)
        {
            existing.AuctionType = AuctionType.AuctionFinished;
            context.Set<AvailableAuction>().Update(existing);
            context.SaveChanges();
        }
    }

    public async Task<AuctionType?> GetAuctionState(Guid auctionId)
    {
        var result = await context.Set<AvailableAuction>().FirstOrDefaultAsync(a => a.AuctionId == auctionId);

        return result?.AuctionType;
    }
}