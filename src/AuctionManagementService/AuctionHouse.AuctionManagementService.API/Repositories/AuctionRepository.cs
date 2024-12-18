using AuctionHouse.AuctionManagementService.Domain.Entities;
using AuctionHouse.AuctionManagementService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.AuctionManagementService.API.Repositories;

public interface IAuctionRepository
{
    Task AddAuction(Auction auction);
    Task<Auction?> GetAuction(Guid auctionId);
    Task<List<Auction>> GetAuctions();
    Task<Guid?> StartAuction(Guid auctionId);
    Task<Guid?> EndAuction(Guid auctionId);
    Task PlaceNewBid(Guid auctionId, int userId, decimal amount);
}

public class AuctionRepository(AuctionDbContext context, ILogger<AuctionRepository> logger) : IAuctionRepository
{
    public async Task AddAuction(Auction auction)
    {
        await context.Auctions.AddAsync(auction);
        await context.SaveChangesAsync();
    }

    public async Task<Auction?> GetAuction(Guid auctionId)
    {
        var auction = await context.Auctions.Include(x=>x.BidSummary).FirstOrDefaultAsync(x => x.AuctionId == auctionId);
        return auction;
    }

    public async Task<List<Auction>> GetAuctions()
    {
        return await context.Auctions.Include(x=>x.BidSummary).ToListAsync();
    }

    public async Task<Guid?> StartAuction(Guid auctionId)
    {
        var auction = await context.Auctions.FirstOrDefaultAsync(x => x.AuctionId == auctionId);
        if (auction != null)
        {
            auction.Status = AuctionStatus.Started;
            auction.StartTime = DateTimeOffset.UtcNow;
            auction.UpdatedAt = DateTimeOffset.UtcNow;
        }

        await context.SaveChangesAsync();

        return auction?.AuctionId;
    }

    public async Task<Guid?> EndAuction(Guid auctionId)
    {
        var auction = await context.Auctions.FirstOrDefaultAsync(x => x.AuctionId == auctionId);
        if (auction != null)
        {
            auction.Status = AuctionStatus.Ended;
            auction.EndTime = DateTimeOffset.UtcNow;
            auction.UpdatedAt = DateTimeOffset.UtcNow;
        }

        await context.SaveChangesAsync();

        return auction?.AuctionId;
    }

    public async Task PlaceNewBid(Guid auctionId, int userId, decimal amount)
    {
        var bidSummary = await context.BidSummaries.FirstOrDefaultAsync(x => x.BidSummaryId == auctionId);
        if (bidSummary != null)
        {
            bidSummary.CurrentHighestBid = amount;
            bidSummary.UserId = userId;
            bidSummary.TotalBids++;
            bidSummary.UpdatedAt = DateTimeOffset.UtcNow;
        }
        else
        {
            logger.LogWarning($"Failed to find auction with auctionId: {auctionId}");
        }
        await context.SaveChangesAsync();
    }
}