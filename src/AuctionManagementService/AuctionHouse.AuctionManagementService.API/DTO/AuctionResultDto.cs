namespace AuctionHouse.AuctionManagementService.API.DTO
{
    public class AuctionResultDto
    {
        public Guid AuctionId { get; set; }
        public Guid ProductId { get; set; }
        public Guid? WinningBidId { get; set; }
        public decimal? WinningAmount { get; set; }
        public bool Success { get; set; }
    }
}
