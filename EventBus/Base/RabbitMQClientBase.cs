using EventBus.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;

namespace EventBus
{
    public abstract class RabbitMQClientBase : IDisposable
    {
        private readonly ILogger _logger;
        protected readonly IOptions<RabbitMQConfiguration> _rabbitMQConfiguration;
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        protected IModel Channel { get; private set; }

        protected RabbitMQClientBase(
            ILogger<RabbitMQClientBase> logger,
            IOptions<RabbitMQConfiguration> rabbitMQConfiguration,
            ConnectionFactory connectionFactory)
        {
            _logger = logger;
            _rabbitMQConfiguration = rabbitMQConfiguration;
            _connectionFactory = connectionFactory;
            SetupRabbitMQ();
        }

        private void SetupRabbitMQ()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = _connectionFactory.CreateConnection();
            }

            if (Channel == null || Channel.IsClosed)
            {
                Channel = _connection.CreateModel();

                Channel.ExchangeDeclare(
                    exchange: _rabbitMQConfiguration.Value.Exchange,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false);

                Channel.QueueDeclare(
                    queue: _rabbitMQConfiguration.Value.Queue,
                    durable: false,
                    exclusive: false,
                    autoDelete: true,
                    null);

                Channel.QueueBind(
                    queue: _rabbitMQConfiguration.Value.Queue,
                    exchange: _rabbitMQConfiguration.Value.Exchange,
                    routingKey: _rabbitMQConfiguration.Value.RoutingKey);
            }
        }

        public void Dispose()
        {
            try
            {
                Channel?.Close();
                Channel?.Dispose();
                Channel = null;

                _connection?.Close();
                _connection?.Dispose();
                _connection = null;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Cannot dispose RabbitMQ channel or connection");
                throw;
            }
        }
    }
}
