namespace EventBus.Configuration
{
    public class RabbitMQConfiguration
    {
        public string Exchange { get; set; }
        public string Queue { get; set; }
        public string RoutingKey { get; set; }
    }
}
