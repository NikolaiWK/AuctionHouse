using System.Text;
using System.Text.Json;
using AuctionHouse.AuctionManagementService.Rabbit.RabbitDtos;
using RabbitMQ.Client;

namespace AuctionHouse.AuctionManagementService.Rabbit;

public class AuctionPublisherService : IAuctionPublisherService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly AuctionPublisherQueueOptions _auctionPublisherQueueOptions;

    public AuctionPublisherService(AuctionPublisherQueueOptions options)
    {
        _auctionPublisherQueueOptions = options;

        var connectionFactory = new ConnectionFactory()
        {
            HostName = _auctionPublisherQueueOptions.HostName,
            Port = _auctionPublisherQueueOptions.Port,
            UserName = _auctionPublisherQueueOptions.Username,
            Password = _auctionPublisherQueueOptions.Password


        };
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: _auctionPublisherQueueOptions.ExchangeName, type: ExchangeType.Topic, durable: true);
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
            exchange: _auctionPublisherQueueOptions.ExchangeName,
            routingKey: _auctionPublisherQueueOptions.RoutingKey,
            basicProperties: null,
            body: body

        );
    }
}