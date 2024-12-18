namespace AuctionHouse.AuctionManagementService.API.DTOs;

public class ProductItemDto
{
    public string title { get; set; }
    public string description { get; set; }
    public int askingPrice { get; set; }
    public string productId { get; set; }
    public bool isSold { get; set; }
}