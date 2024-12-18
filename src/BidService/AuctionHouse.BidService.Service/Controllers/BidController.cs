using AuctionHouse.BidService.Service.DTOs;
using AuctionHouse.BidService.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.BidService.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BidController(IBidService bidService) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> BidOnAuction(BidDto bidDto)
        {
            var bid = await bidService.PlaceBid(bidDto.AuctionId, bidDto.BidAmount, bidDto.UserId);
            if (bid == null)
            {
                return NotFound();
            }
            return Ok(bid);
        }
    }
}
