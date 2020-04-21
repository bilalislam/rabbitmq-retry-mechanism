using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace Bus.RabbitMq
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRabbitMqBus(this IServiceCollection services)
        {
            services.AddSingleton<IPooledObjectPolicy<IConnection>, ConnectionPooledObjectPolicy>();
            services.AddSingleton<IDispatchedEventBus, DispatchedEventBus>();
            return services;
        }
    }
}