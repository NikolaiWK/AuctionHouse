using AuctionHouse.AuctionManagementService.API.DTO;
using AuctionHouse.AuctionManagementService.API.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.AuctionManagementService.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AuctionController : ControllerBase
    {

        private readonly ILogger<AuctionController> _logger;
        private readonly IAuctionService _auctionService;

        public AuctionController(ILogger<AuctionController> logger, IAuctionService auctionService)
        {
            _logger = logger;
            _auctionService = auctionService;
        }

        
        [HttpPost("CreateAuction")]
        public async Task<IActionResult> CreateAuction(CreateAuctionDto auctionDto)
        {
            _logger.LogInformation($"Creating auction for product {auctionDto.ProductId}");

            var auctionId = await _auctionService.CreateAuction(auctionDto);
            if (auctionId == null)
            {
                return BadRequest("Auction could not be created.");
            }

            return Ok(auctionId);
        }



        [HttpGet("{auctionId:guid}")]
        public async Task<IActionResult> GetAuction(Guid auctionId)
        {
            _logger.LogInformation($"Fetching auction with ID {auctionId}");

            var auction = await _auctionService.GetAuction(auctionId);
            if (auction == null)
            {
                return NotFound("Auction not found.");
            }

            return Ok(auction);
        }


     

       
        [HttpGet]
        public async Task<IActionResult> GetActiveAuctions()
        {
            _logger.LogInformation("Fetching all active auctions");

            var auctions = await _auctionService.GetActiveAuctions();
            return Ok(auctions);
        }

        [HttpDelete("{auctionId:guid}")]
        public async Task<IActionResult> DeleteAuction(Guid auctionId)
        {
            _logger.LogInformation("Delete auction");

            var auction = await _auctionService.DeleteAuction(auctionId);

            if (auction == null)
            {
                return NotFound("Auction not found");
            }

            return NoContent();
        }

    }

}
