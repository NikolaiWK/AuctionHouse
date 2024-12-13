using AuctionHouse.CatalogService.Domain.Entities;
using AuctionHouse.CatalogService.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.CatalogService.API.Services;

/// <summary>
/// Interface definition for the DB service to access the catalog data.
/// </summary>
public interface ICatalogService
{
    Task<List<ProductItem>> GetProductItems();
    Task<ProductItem?> GetProductItem(Guid productId);
    Task<Guid?> AddProductItem(ProductItem item);
    Task ProductSold(Guid productId);
    Task DeleteItem(Guid productId);
}

/// <summary>
/// MongoDB repository service
/// </summary>
public class CatalogService : ICatalogService
{
    private ILogger<CatalogService> _logger;
    private IConfiguration _config;
    private MongoDbContext _context;


    /// <param name="logger">The commun logger facility instance</param>
    /// <param name="config">Systemm configuration instance</param>
    /// <param name="dbcontext">The database context to be used for accessing data.</param>
    public CatalogService(ILogger<CatalogService> logger,
            IConfiguration config, MongoDbContext dbContext)
    {
        _logger = logger;
        _config = config;
        _context = dbContext;
    }

    public async Task<List<ProductItem>> GetProductItems()
    {
        try
        {
            return await _context.Products.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    /// <summary>
    /// Retrieves a product item based on its unique id.
    /// </summary>
    /// <param name="productId">The products unique id</param>
    /// <returns>The products item requested.</returns>
    public async Task<ProductItem?> GetProductItem(Guid productId)
    {
        try
        {
            return await _context.Products.SingleOrDefaultAsync(x => x.ProductId == productId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }


    /// <summary>
    /// Add a new Product Item to the database.
    /// </summary>
    /// <param name="item">Product to add to the catalog</param>
    /// <returns>Product with updated Id</returns>
    public async Task<Guid?> AddProductItem(ProductItem item)
    {
        try
        {
            await _context.Products.AddAsync(item);
            await _context.SaveChangesAsync();
            return item.ProductId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task ProductSold(Guid productId)
    {
        try
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.ProductId == productId);
            if (product == null)
            {
                _logger.LogWarning($"Product with ID {productId} not found.");
                return;
            }

            product.IsSold = true;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task DeleteItem(Guid productId)
    {
        try
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.ProductId == productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Product with ID {productId} not found.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}