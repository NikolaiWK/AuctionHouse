using AuctionHouse.AuctionManagementService.API.DTO;

namespace AuctionHouse.AuctionManagementService.API.Interface
{
    public interface IAuctionService
    {
        Task<Guid?> CreateAuction(CreateAuctionDto auctionDto);
        Task<AuctionDto?> GetAuction(Guid auctionId);
        Task<IEnumerable<AuctionDto>> GetActiveAuctions();
        Task<bool> DeleteAuction(Guid auctionId);
    }
}
