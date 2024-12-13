using AuctionHouse.AuctionManagementService.API.Entity;

namespace AuctionHouse.AuctionManagementService.API.Services
{
    public interface IAuctionRepository
    {
        Task AddAuction(Auction auction);
        Task<Auction?> GetAuction(Guid auctionId);
        Task<IEnumerable<Auction>> GetActiveAuctions();
        Task UpdateAuction(Auction auction);
        Task DeleteAuction(Guid auctionId);
    }
}
