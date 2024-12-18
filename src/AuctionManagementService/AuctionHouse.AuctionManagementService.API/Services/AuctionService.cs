using System.Text.Json;
using AuctionHouse.AuctionManagementService.API.DTOs;
using AuctionHouse.AuctionManagementService.API.Repositories;
using AuctionHouse.AuctionManagementService.Domain.Entities;
using AuctionHouse.AuctionManagementService.Rabbit;
using AuctionHouse.AuctionManagementService.Rabbit.RabbitDtos;

namespace AuctionHouse.AuctionManagementService.API.Services;

public interface IAuctionService
{
    Task<Guid?> CreateAuction(CreateAuctionDto auctionDto, ProductItemDto productDto);
    Task<AuctionDto?> GetAuction(Guid actionId);
    Task<List<AuctionDto>> GetAuctions();
    Task<Guid?> StartAuction(Guid auctionId);
    Task<Guid?> EndAuction(Guid auctionId);
}

public class AuctionService(IAuctionRepository repository, ILogger<AuctionService> logger, IAuctionPublisherService publisherService) : IAuctionService
{
    public async Task<Guid?> CreateAuction(CreateAuctionDto dto, ProductItemDto productDto)
    {
        logger.LogInformation("Creating Auction from auctionDto and productDto");

        var auctionId = Guid.NewGuid();
        var auction = new Auction
        {
            AuctionId = auctionId, 
            Name = productDto.title,
            Description = productDto.description,
            StartingPrice = productDto.askingPrice,
            Status = AuctionStatus.Created,
            StartTime = dto.StartTime.ToUniversalTime(),
            EndTime = dto.EndTime.ToUniversalTime(),
            BidSummary = new BidSummary
            {
                BidSummaryId = auctionId,
                CurrentHighestBid = 0,
                TotalBids = 0,
                UpdatedAt = DateTimeOffset.UtcNow,
                UserId = null
            },
            ProductId = Guid.Parse(productDto.productId),
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };

        await repository.AddAuction(auction);

        return auctionId;
    }

    public async Task<AuctionDto?> GetAuction(Guid actionId)
    {
        logger.LogInformation($"Fetcing Auction data for {actionId}");

        var result = await repository.GetAuction(actionId);
        if (result != null)
        {
            return new AuctionDto
            {
                AuctionId = result.AuctionId,
                Description = result.Description,
                StartTime = result.StartTime,
                EndTime = result.EndTime,
                Name = result.Name,
                ProductId = result.ProductId,
                StartingPrice = result.StartingPrice,
                Status = (AuctionStatusDto)result.Status,
                BidSummary = new BidSummaryDto
                {
                    CurrentHighestBid = result.BidSummary.CurrentHighestBid,
                    TotalBids = result.BidSummary.TotalBids,
                    UserId = result.BidSummary.UserId
                }
            };
        }

        return null;

    }

    public async Task<List<AuctionDto>> GetAuctions()
    {
        var result = await repository.GetAuctions();

        return result.Select(x => new AuctionDto
        {
            AuctionId = x.AuctionId,
            BidSummary = new BidSummaryDto
            {
                CurrentHighestBid = x.BidSummary.CurrentHighestBid,
                TotalBids = x.BidSummary.TotalBids,
                UserId = x.BidSummary.UserId,
            },
            StartingPrice = x.StartingPrice,
            Status = (AuctionStatusDto)x.Status,
            Description = x.Description,
            EndTime = x.EndTime,
            Name = x.Name,
            ProductId = x.ProductId,
            StartTime = x.StartTime
        }).ToList();
    }

    public async Task<Guid?> StartAuction(Guid auctionId)
    {
        var result = await repository.StartAuction(auctionId);
        if (result != null)
        {
            var messageBody = JsonSerializer.Serialize(new AuctionBaseEvent()
            {
                AuctionType = AuctionType.AuctionStarted,
                AuctionId = (Guid)result
            });

            publisherService.PublishMessage(messageBody);
        }

        return result;
    }

    public async Task<Guid?> EndAuction(Guid auctionId)
    {
        //Send event on to bid-service that auction has ended.
        //Request item sold to catalog-service.
        //Request payment service for userId and amount.
        //Notify involved users that auction has ended.

        var result = await repository.EndAuction(auctionId);
        if (result != null)
        {
            var messageBody = JsonSerializer.Serialize(new AuctionBaseEvent()
            {
                AuctionType = AuctionType.AuctionFinished,
                AuctionId = (Guid)result
            });

            publisherService.PublishMessage(messageBody);
        }

        return result;
    }
}