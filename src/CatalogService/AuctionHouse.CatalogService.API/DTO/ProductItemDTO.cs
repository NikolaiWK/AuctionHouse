using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AuctionHouse.CatalogService.API.DTO;



public class ProductItemDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal AskingPrice { get; set; }
    public List<Uri> Images { get; set; } = new List<Uri>();
    public Guid? ProductId { get; set; }
    public bool IsSold { get; set; }

    // public string Currency {get; set;} add For future scaling
    
}