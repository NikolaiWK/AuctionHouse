namespace AuctionHouse.AuctionManagementService.API.DTO
{
    public class CreateAuctionDto
    {
        public Guid ProductId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
