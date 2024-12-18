using System.Text;
using RabbitMQ.Client;

namespace AuctionHouse.BidService.Rabbit;

public class BidPublisherService : IBidPublisherService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly BidPublisherQueueOptions _bidPublisherQueueOptions;

    public BidPublisherService(BidPublisherQueueOptions options)
    {
        _bidPublisherQueueOptions = options;

        var connectionFactory = new ConnectionFactory()
        {
            HostName = _bidPublisherQueueOptions.HostName,
            Port = _bidPublisherQueueOptions.Port,
            UserName = _bidPublisherQueueOptions.Username,
            Password = _bidPublisherQueueOptions.Password


        };
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: _bidPublisherQueueOptions.ExchangeName, type: ExchangeType.Topic, durable: true);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }

    public void PublishMessage(string bidEvent)
    {
        var body = Encoding.UTF8.GetBytes(bidEvent);

        _channel.BasicPublish(
            exchange: _bidPublisherQueueOptions.ExchangeName,
            routingKey: _bidPublisherQueueOptions.RoutingKey,
            basicProperties: null,
            body: body

        );
    }
}