using EventBus.Abstractions;
using EventBus.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace EventBus
{
    public class RabbitMQProducer<T> : RabbitMQClientBase, IRabbitProducer<T>
    {
        public RabbitMQProducer(
            ILogger<RabbitMQClientBase> logger, 
            IOptions<RabbitMQConfiguration> rabbitMQConfiguration, 
            ConnectionFactory connectionFactory
        ) : base(logger, rabbitMQConfiguration, connectionFactory)
        { }

        public void Publish(T @event)
        {
            var eventSerialized = JsonConvert.SerializeObject(@event);
            var eventBytes = Encoding.UTF8.GetBytes(eventSerialized);

            var props = Channel.CreateBasicProperties();

            Channel.BasicPublish(
                exchange: _rabbitMQConfiguration.Value.Exchange,
                routingKey: _rabbitMQConfiguration.Value.RoutingKey,
                basicProperties: props,
                body: eventBytes);
        }
    }
}
