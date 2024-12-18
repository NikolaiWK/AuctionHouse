using AuctionHouse.BidService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.BidService.Infrastructure.DbContext;

public class BidDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public BidDbContext(DbContextOptions<BidDbContext> options) : base(options)
    {
    }

    public DbSet<AvailableAuction> AvailableAuctions { get; set; }
    public DbSet<Bid> Bids { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AvailableAuction>()
            .HasKey(x => x.AuctionId);
    }
}
