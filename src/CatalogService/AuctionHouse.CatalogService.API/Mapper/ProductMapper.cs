using System.Net.NetworkInformation;
using AuctionHouse.CatalogService.API.DTO;
using AuctionHouse.CatalogService.Domain.Entities;

namespace AuctionHouse.CatalogService.API.Mapper
{
    public static class ProductMapper
    {
        public static ProductItem MapToProduct(ProductItemDTO productItemDTO)
        {
            return new ProductItem
            {
                ProductId = Guid.NewGuid(),
                AskingPrice = productItemDTO.AskingPrice,
                CreatedAt = DateTime.Now,
                Description = productItemDTO.Description,
                Images = productItemDTO.Images,
                Title = productItemDTO.Title,
                UpdatedAt = DateTime.Now
            };
        }

        public static ProductItemDTO MapToProductDTO(ProductItem productItem)
        {
            return new ProductItemDTO
            {
                Title = productItem.Title,
                Description = productItem.Description,
                AskingPrice = productItem.AskingPrice,
                Images = productItem.Images,
                IsSold = productItem.IsSold,
                ProductId = productItem.ProductId
                 
            };

        }
    }
}
