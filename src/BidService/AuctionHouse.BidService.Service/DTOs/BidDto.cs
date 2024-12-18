namespace AuctionHouse.BidService.Service.DTOs;

public class BidDto
{
    public required Guid AuctionId { get; set; }
    public required decimal BidAmount { get; set; }
    public required int UserId { get; set; }
}