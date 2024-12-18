using System.Text;
using System.Text.Json;
using AuctionHouse.BidService.Service.RabbitDtos;
using AuctionHouse.BidService.Service.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AuctionHouse.BidService.Service;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly QueueOptions.BidReceiverQueueOptions _bidReceiverQueueOptions;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private EventingBasicConsumer? _consumer;

    public Worker(ILogger<Worker> logger, QueueOptions.BidReceiverQueueOptions options, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _bidReceiverQueueOptions = options;
        _scopeFactory = scopeFactory;

        var connectionFactory = new ConnectionFactory()
        {
            HostName = _bidReceiverQueueOptions.HostName,
            Port = _bidReceiverQueueOptions.Port,
            UserName = _bidReceiverQueueOptions.Username,
            Password = _bidReceiverQueueOptions.Password
        };
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: _bidReceiverQueueOptions.ExchangeName, type: ExchangeType.Topic, durable: true);
        _channel.QueueDeclare(queue: _bidReceiverQueueOptions.QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(queue: _bidReceiverQueueOptions.QueueName, exchange: _bidReceiverQueueOptions.ExchangeName, routingKey: _bidReceiverQueueOptions.RoutingKey);

        // Set QoS to limit unacknowledged messages
        _channel.BasicQos(0, prefetchCount: 1, global: false);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Starting RabbitMQ consumer...");

        // Initialize consumer
        _consumer = new EventingBasicConsumer(_channel);

        _consumer.Received += (ch, ea) =>
        {
            try
            {
                // Process the message
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var auctionEvent = JsonSerializer.Deserialize<AuctionBaseEvent>(message);

                if (auctionEvent != null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var scopedEventService =
                        scope.ServiceProvider
                            .GetRequiredService<IEventService>();

                    scopedEventService.ConsumeEvent(auctionEvent);
                    _logger.LogInformation("Message processed successfully.");
                }
                else
                {
                    _logger.LogError("Failed to deserialize auction event.");
                    throw new Exception("Failed to deserialize to auction event...");
                }

                // Acknowledge message
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message. Requeuing message...");
                _channel.BasicNack(ea.DeliveryTag, false, true); // Requeue message
            }
        };

        // Start consuming messages
        _channel.BasicConsume(queue: _bidReceiverQueueOptions.QueueName, autoAck: false, consumer: _consumer);
        return Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
        base.Dispose();
    }
}