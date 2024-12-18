using System.Net;
using AuctionHouse.AuctionManagementService.API.DTOs;
using AuctionHouse.AuctionManagementService.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Text.Json;

namespace AuctionHouse.AuctionManagementService.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[Controller]")]
    public class AuctionController(ILogger<AuctionController> logger, IAuctionService auctionService, IHttpClientFactory httpClientFactory) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> CreateAuction(CreateAuctionDto auctionDto)
        {
            logger.LogInformation($"Creating auction for product {auctionDto.ProductId}");
            
            var httpRequestMessage = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://catalog-service:6051/Catalog/{auctionDto.ProductId}")
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/vnd.github.v3+json" },
                    { HeaderNames.UserAgent, "HttpRequestsSample" },
                    { HeaderNames.Authorization, Request.Headers.Authorization.ToString() }
                }
            };

            var httpClient = httpClientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

            ProductItemDto? product = null;
            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {
                await using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                product = await JsonSerializer.DeserializeAsync
                    <ProductItemDto>(contentStream);
            }

            if (product == null)
            {
                return NotFound("Couldn't find product in catalog");
            }

            if (product.isSold)
            {
                return BadRequest("Auction could not be created. Product is already sold on auction.");
            }

            var auctionId = await auctionService.CreateAuction(auctionDto, product);
            if (auctionId == null)
            {
                return BadRequest("Auction could not be created.");
            }

            return Ok(auctionId);
        }

        //This is a mock endpoint to simulate that the StartTime has been reached for an auction - This is due to the AuctionTimerJob not being implemented...
        [HttpPut("start/{auctionId:guid}")]
        public async Task<IActionResult> StartAuction(Guid auctionId)
        {
            var auction = await auctionService.StartAuction(auctionId);

            if (auction!=null)
            {
                return Ok(auction);
            }

            return NotFound(auction);
        }


        //This is a mock endpoint to simulate that the EndTime has been reached for an auction - This is due to the AuctionTimerJob not being implemented...
        [HttpPut("end/{auctionId:guid}")]
        public async Task<IActionResult> EndAuction(Guid auctionId)
        {
            var auction = await auctionService.EndAuction(auctionId);

            if (auction != null)
            {
                return Ok(auction);
            }

            return NotFound(auction);
        }

        [HttpGet("{auctionId:guid}")]
        public async Task<IActionResult> GetAuction(Guid auctionId)
        {
            logger.LogInformation($"Fetching auction with ID {auctionId}");

            var auction = await auctionService.GetAuction(auctionId);
            if (auction == null)
            {
                return NotFound("Auction not found.");
            }

            return Ok(auction);
        }
    }
}
