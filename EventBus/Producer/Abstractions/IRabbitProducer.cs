namespace EventBus.Abstractions
{
    public interface IRabbitProducer<in T>
    {
        void Publish(T @event);
    }
}
