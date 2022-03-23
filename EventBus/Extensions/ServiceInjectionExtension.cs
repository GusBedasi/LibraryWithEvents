using Domain;
using EventBus.Abstractions;
using EventBus.Consumer;
using EventBus.Consumer.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBus.Extensions
{
    public static class ServiceInjectionExtension
    {
        public static void AddProducer(this IServiceCollection services)
        {
            services.AddScoped<IRabbitProducer<Book>, RabbitMQProducer<Book>>();
            services.AddScoped<IRabbitConsumer, RabbitMQConsumer>();
        }
    }
}
