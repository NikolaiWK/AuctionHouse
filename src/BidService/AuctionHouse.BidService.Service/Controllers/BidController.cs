
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.BidService.Service.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class BidController
    {
        
    }
}
