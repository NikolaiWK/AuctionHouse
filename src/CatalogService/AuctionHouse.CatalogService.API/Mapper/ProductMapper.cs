using AuctionHouse.CatalogService.API.DTO;
using AuctionHouse.CatalogService.Domain.Entities;

namespace AuctionHouse.CatalogService.API.Mapper
{
    public static class ProductMapper
    {
        public static ProductItem MapToProduct(ProductItemDto productItemDTO)
        {
            return new ProductItem
            {
                ProductId = Guid.NewGuid(),
                AskingPrice = productItemDTO.AskingPrice,
                CreatedAt = DateTime.Now,
                Description = productItemDTO.Description,
                Title = productItemDTO.Title,
                UpdatedAt = DateTime.Now
            };
        }

        public static ProductItemDto MapToProductDTO(ProductItem productItem)
        {
            return new ProductItemDto
            {
                Title = productItem.Title,
                Description = productItem.Description,
                AskingPrice = productItem.AskingPrice,
                IsSold = productItem.IsSold,
                ProductId = productItem.ProductId
                 
            };

        }
    }
}
