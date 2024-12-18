using AuctionHouse.CatalogService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using System.Xml;
using System;

namespace AuctionHouse.CatalogService.Infrastructure.DbContext;

/// <summary>
/// MongoDB database context class.
/// </summary>
public class MongoDbContext(DbContextOptions<MongoDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<ProductItem> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductItem>(entity =>
        {
            entity.HasKey(e => e.ProductId);
            
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
        });
        base.OnModelCreating(modelBuilder);
    }
}