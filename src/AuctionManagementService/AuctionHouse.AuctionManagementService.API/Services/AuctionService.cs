using AuctionHouse.AuctionManagementService.API.DTO;
using AuctionHouse.AuctionManagementService.API.Entity;
using AuctionHouse.AuctionManagementService.API.Services;

namespace AuctionHouse.AuctionManagementService.API.Interface
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _repository;
       

        public AuctionService(IAuctionRepository repository)
        {
            _repository = repository;
            
        }

        public async Task<Guid?> CreateAuction(CreateAuctionDto auctionDto)
        {
            var auction = new Auction
            {
                AuctionId = Guid.NewGuid(),
                ProductId = auctionDto.ProductId,
                StartTime = auctionDto.StartTime,
                EndTime = auctionDto.EndTime,
                IsActive = true
            };

            await _repository.AddAuction(auction);
            return auction.AuctionId;
        }

        public async Task<AuctionDto?> GetAuction(Guid auctionId)
        {
            var auction = await _repository.GetAuction(auctionId);
            if (auction == null) return null;

            return new AuctionDto
            {
                AuctionId = auction.AuctionId,
                ProductId = auction.ProductId,
                StartTime = auction.StartTime,
                EndTime = auction.EndTime,
                IsActive = auction.IsActive
            };
        }


        public async Task<bool> DeleteAuction(Guid auctionId)
        {
            
            var auction = await _repository.GetAuction(auctionId);
            if (auction == null)
            {
                return false; 
            }

           
            await _repository.DeleteAuction(auctionId);
            return true;
        }

        public async Task<IEnumerable<AuctionDto>> GetActiveAuctions()
        {
            var auctions = await _repository.GetActiveAuctions();
            return auctions.Select(a => new AuctionDto
            {
                AuctionId = a.AuctionId,
                ProductId = a.ProductId,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                IsActive = a.IsActive
            });
        }

       
    }

}
