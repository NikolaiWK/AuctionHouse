
namespace AuctionHouse.AuctionManagementService.API.DTOs;

public class AuctionDto
{
    public Guid AuctionId { get; set; }
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal StartingPrice { get; set; }
    public AuctionStatusDto Status { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    public BidSummaryDto BidSummary { get; set; }

}

public class BidSummaryDto
{
    public decimal CurrentHighestBid { get; set; }
    public int TotalBids { get; set; }
}

public enum AuctionStatusDto
{
    Created = 0,
    Started = 1,
    Ended = 2,
}