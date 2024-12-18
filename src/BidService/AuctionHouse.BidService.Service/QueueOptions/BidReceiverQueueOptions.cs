namespace AuctionHouse.BidService.Service.QueueOptions;

public class BidReceiverQueueOptions
{
    public string HostName { get; set; }
    public string ExchangeName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string RoutingKey { get; set; }
    public int Port { get; set; } = 5672;
    public string QueueName { get; set; }

}