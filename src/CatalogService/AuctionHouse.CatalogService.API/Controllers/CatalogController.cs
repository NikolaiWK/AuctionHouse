using AuctionHouse.CatalogService.API.DTO;
using AuctionHouse.CatalogService.API.Mapper;
using AuctionHouse.CatalogService.API.Services;
using AuctionHouse.CatalogService.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouse.CatalogService.API.Controllers;

/// <summary>
/// The CatalogController implements the HTTP interface for accessing
/// the product items catalog from a food business.
/// </summary>
[ApiController]
[Authorize]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;
    private string _imagePath = string.Empty;
    private readonly ICatalogService _dbService;

    /// <summary>
    /// Create an instance of the Catalog controller.
    /// </summary>
    /// <param name="logger">Global logging instance</param>
    /// <param name="configuration">Global configuration instance</param>
    /// <param name="dbservice">Database respository</param>
    public CatalogController(ILogger<CatalogController> logger,
        IConfiguration configuration, ICatalogService dbservice)
    {
        _logger = logger;
        _imagePath = configuration["CatalogImagePath"]!;
        _dbService = dbservice;
    }

    /// <summary>
    /// Service version endpoint. 
    /// Fetches metadata information, through reflection from the service assembly.
    /// </summary>
    /// <returns>All metadata attributes from assembly in text string</returns>
    [HttpGet("version")]
    public Dictionary<string, string> GetVersion()
    {
        var properties = new Dictionary<string, string>();
        var assembly = typeof(Program).Assembly;

        properties.Add("service", "Catalog");
        var ver = System.Diagnostics.FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion ?? "Undefined";
        properties.Add("version", ver);

        var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
        var localIPAddr = feature?.LocalIpAddress?.ToString() ?? "N/A";
        properties.Add("local-host-address", localIPAddr);

        return properties;
    }

    [HttpGet("{productId:guid}")]
    public async Task<ProductItemDto?> GetProduct(Guid productId)
    {
        _logger.LogInformation($"Request for product with guid: {productId}");

        var result = await _dbService.GetProductItem(productId);

        if (result==null)
        {
            return null;
        }

        return ProductMapper.MapToProductDTO(result);
    }

    [HttpGet]
    public async Task<List<ProductItemDto>> GetProducts()
    {
        _logger.LogInformation("Request for all products");

        var result = await _dbService.GetProductItems();

        return result.Select(ProductMapper.MapToProductDTO).ToList();
    }

    [HttpPost("CreateProduct")]
    public Task<Guid?> CreateProduct(ProductItemDto dto)
    {
        var createdProduct = ProductMapper.MapToProduct(dto);

        return _dbService.AddProductItem(createdProduct);
    }

    [HttpPut("{productId:guid}")]
    public async Task<Guid?> ProductSold(Guid productId)
    {
        await _dbService.ProductSold(productId);
        return productId;
    }

    [HttpDelete("{productId:guid}")]
    public async Task<Guid?> DeleteProduct(Guid productId)
    {
        await _dbService.DeleteItem(productId);
        return productId;
    }


    //[HttpPost("AddImage"), DisableRequestSizeLimit]
    //public async Task<IActionResult> UploadImage()
    //{
    //    List<Uri> images = new List<Uri>();

    //    try
    //    {
    //        var formId = Request.Form["guid"];

    //        if (String.IsNullOrEmpty(formId))
    //        {
    //            return BadRequest("The product id could not be identified.");
    //        }

    //        Guid productId = new Guid(formId!);
    //        ProductItemDTO? result = await _dbService.GetProductItem(productId);

    //        if (result != null)
    //        {
    //            foreach (var formFile in Request.Form.Files)
    //            {
    //                if (formFile.Length > 0)
    //                {
    //                    var fileName = "image-" + Guid.NewGuid().ToString() + ".jpg";
    //                    var fullPath = _imagePath + Path.DirectorySeparatorChar + fileName;
    //                    _logger.LogInformation($"Saving file {fullPath}");

    //                    using (var stream = new FileStream(fullPath, FileMode.Create))
    //                    {
    //                        formFile.CopyTo(stream);
    //                    }

    //                    var imageURI = new Uri(fileName, UriKind.RelativeOrAbsolute);

    //                    if (await _dbService.AddImageToProductItem(productId, imageURI) > 0)
    //                    {
    //                        images.Add(imageURI);
    //                    }
    //                }
    //                else
    //                {
    //                    return BadRequest("Empty file submited.");
    //                }
    //            }
    //        }
    //        else
    //        {
    //            return StatusCode(404, $"Product not found in catalog");
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError("Upload image faillure {}", ex);
    //        return StatusCode(500, $"Internal server error.");
    //    }

    //    return Ok(images);
    //}

}