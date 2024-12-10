using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using AuctionHouse.CatalogService.Domain.Entities;
using AuctionHouse.CatalogService.API.DTO;

namespace Catalog.Services;

/// <summary>
/// Interface definition for the DB service to access the catalog data.
/// </summary>
public interface ICatalogService
{
    Task<ProductItem?> GetProductItem(Guid productId);
    Task<Guid?> AddProductItem(ProductItem item);
    Task<long> AddImageToProductItem(Guid productId, Uri imageURI);
}

/// <summary>
/// MongoDB repository service
/// </summary>
public class CatalogService : ICatalogService
{
    private ILogger<CatalogService> _logger;
    private IConfiguration _config;
    private IMongoDatabase _database;
    private IMongoCollection<ProductItem> _collection;

    
    /// <param name="logger">The commun logger facility instance</param>
    /// <param name="config">Systemm configuration instance</param>
    /// <param name="dbcontext">The database context to be used for accessing data.</param>
    public CatalogService(ILogger<CatalogService> logger,
            IConfiguration config, MongoDBContext dbcontext)
    {
        _logger = logger;
        _config = config;
        _database = dbcontext.Database;
        _collection = dbcontext.Collection;
    }

    /// <summary>
    /// Retrieves a product item based on its unique id.
    /// </summary>
    /// <param name="productId">The products unique id</param>
    /// <returns>The products item requested.</returns>
    public async Task<ProductItem?> GetProductItem(Guid productId)
    {
        ProductItem? product = null;
        var filter = Builders<ProductItem>.Filter.Eq(x => x.ProductId, productId);

        try
        {
            product = await _collection.Find(filter).SingleOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return product;
    }

   
    /// <summary>
    /// Add a new Product Item to the database.
    /// </summary>
    /// <param name="item">Product to add to the catalog/param>
    /// <returns>Product with updated Id</returns>
    public async Task<Guid?> AddProductItem(ProductItem item)
    {
        
        await _collection.InsertOneAsync(item);
        return item.ProductId;
    }

    /// <summary>
    /// Append an image URI to the Images list in a ProductItem and persists to database.
    /// </summary>
    /// <param name="productId">The products unique ID</param>
    /// <param name="uri">the absolute URI of the image</param>
    /// <returns>Number of items updated.</returns>
    public async Task<long> AddImageToProductItem(Guid productId, Uri uri)
    {
        var filter = Builders<ProductItem>.Filter.Eq("_id", productId.ToString());
        var update = Builders<ProductItem>.Update.AddToSet("Images", uri);

        var res = await _collection.UpdateOneAsync(filter, update);

        return res.ModifiedCount;
    }

}