using AuctionHouse.BidService.Domain.Entities;
using AuctionHouse.BidService.Infrastructure.DbContext;

namespace AuctionHouse.BidService.Service.Repositories;

public interface IBidRepository
{
    public void CreateBid(Bid bid);

}

public class BidRepository(BidDbContext context) : IBidRepository
{
    public void CreateBid(Bid bid)
    {
        context.Set<Bid>().Add(bid);
        context.SaveChanges();
    }
}