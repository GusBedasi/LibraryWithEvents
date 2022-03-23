using EventBus.Configuration;
using EventBus.Consumer.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Consumer
{
    public class RabbitMQConsumer : RabbitMQClientBase, IRabbitConsumer
    {
        private readonly ILogger _logger;
        public RabbitMQConsumer(
            ILogger<RabbitMQClientBase> logger, 
            IOptions<RabbitMQConfiguration> rabbitMQConfiguration, 
            ConnectionFactory connectionFactory
        ) : base(logger, rabbitMQConfiguration, connectionFactory)
        { 
            _logger = logger;
        }

        public void Consumer()
        {
            var consumer = new EventingBasicConsumer(Channel);

            consumer.Received += (model, ea) =>
            {
                try
                {
                    var messageBytes = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(messageBytes);

                    _logger.LogInformation(message);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Could not consume the message");
                    Channel.BasicNack(ea.DeliveryTag, false, true);
                }
                finally
                {
                    Channel.BasicAck(ea.DeliveryTag, false);
                }
            };
        }
    }
}
