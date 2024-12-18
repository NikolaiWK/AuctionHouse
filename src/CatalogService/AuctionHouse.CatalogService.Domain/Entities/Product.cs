using MongoDB.Bson.Serialization.Attributes;

namespace AuctionHouse.CatalogService.Domain.Entities
{
   
    public class ProductItem
    {
        [BsonId]
        [BsonElement("_id")]
        public Guid? ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal AskingPrice { get; set; }
        // public string Currency {get; set;} add For future scaling
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get;set; }
        public bool IsSold { get; set; }

    }
}
