namespace AuctionHouse.CatalogService.API.DTO;



public class ProductItemDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal AskingPrice { get; set; }
    public Guid? ProductId { get; set; }
    public bool IsSold { get; set; }

    // public string Currency {get; set;} add For future scaling
    
}