using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.AuctionManagementService.API.DTOs;

public class CreateAuctionDto
{
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public DateTimeOffset StartTime { get; set; }
    [Required]
    public DateTimeOffset EndTime { get; set; }
}