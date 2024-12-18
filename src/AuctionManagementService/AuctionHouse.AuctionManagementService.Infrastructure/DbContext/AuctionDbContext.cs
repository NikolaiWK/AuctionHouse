using AuctionHouse.AuctionManagementService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.AuctionManagementService.Infrastructure.DbContext;

public class AuctionDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options)
    {
    }

    public DbSet<Auction> Auctions { get; set; }
    public DbSet<BidSummary> BidSummaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Auction>()
            .HasOne(a => a.BidSummary)        // Auction has one BidSummary
            .WithOne(b => b.Auction)          // BidSummary belongs to one Auction
            .HasForeignKey<BidSummary>(b => b.AuctionId) // BidSummary uses AuctionId as FK
            .OnDelete(DeleteBehavior.Cascade);

        // Configure AuctionId as PK for BidSummary
        modelBuilder.Entity<BidSummary>()
            .Property(b => b.BidSummaryId)
            .ValueGeneratedNever();
    }
}