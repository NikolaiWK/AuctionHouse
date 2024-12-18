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
                }
            };
        }

        return null;

    }

    public async Task<Guid?> StartAuction(Guid auctionId)
    {
        var result = await repository.StartAuction(auctionId);
        if (result != null)
        {
            var messageBody = JsonSerializer.Serialize(new AuctionStartedEvent
            {
                AuctionType = AuctionType.AuctionStarted,
                AuctionId = (Guid)result,
                StartTime = DateTimeOffset.UtcNow
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
            var messageBody = JsonSerializer.Serialize(new AuctionEndedEvent()
            {
                AuctionType = AuctionType.AuctionFinished,
                AuctionId = (Guid)result,
                EndTime = DateTimeOffset.UtcNow
            });

            publisherService.PublishMessage(messageBody);
        }

        return result;
    }
}