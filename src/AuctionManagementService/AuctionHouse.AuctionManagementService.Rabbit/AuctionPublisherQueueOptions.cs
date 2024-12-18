namespace AuctionHouse.AuctionManagementService.Rabbit
{
    public class AuctionPublisherQueueOptions
    {
        public string HostName { get; set; }
        public string ExchangeName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RoutingKey { get; set; }
        public int Port { get; set; } = 5672;

    }
}
