﻿namespace AuctionHouse.AuctionManagementService.API.DTO
{
    public class AuctionDto
    {
        public Guid AuctionId { get; set; }
        public Guid ProductId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }
    }
}
