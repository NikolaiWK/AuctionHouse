using System.Text;
using System.Text.Json;
using AuctionHouse.AuctionManagementService.Rabbit.RabbitDtos;
using RabbitMQ.Client;

namespace AuctionHouse.AuctionManagementService.Rabbit;

public class AuctionPublisherService : IAuctionPublisherService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly QueueOptions _queueOptions;

    public AuctionPublisherService(QueueOptions options)
    {
        _queueOptions = options;

        var connectionFactory = new ConnectionFactory()
        {
            HostName = _queueOptions.HostName,
            Port = _queueOptions.Port,
            UserName = _queueOptions.Username,
            Password = _queueOptions.Password


        };
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: _queueOptions.ExchangeName, type: ExchangeType.Topic, durable: true);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }

    public void PublishMessage(string auctionEvent)
    {
        var body = Encoding.UTF8.GetBytes(auctionEvent);

        _channel.BasicPublish(
            exchange: _queueOptions.ExchangeName,
            routingKey: _queueOptions.RoutingKey,
            basicProperties: null,
            body: body

        );
    }
}