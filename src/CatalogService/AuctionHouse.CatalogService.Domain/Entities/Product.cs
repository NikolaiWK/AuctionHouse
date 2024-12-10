using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace AuctionHouse.CatalogService.Domain.Entities
{
   
    [BsonDiscriminator("productitem")]
    public class ProductItem
    {
        [BsonId]
        public Guid? ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal AskingPrice { get; set; }
        public List<Uri> Images { get; set; } = new List<Uri>();
        // public string Currency {get; set;} add For future scaling
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get;set; }
        public bool IsSold { get; set; }

    }
}
