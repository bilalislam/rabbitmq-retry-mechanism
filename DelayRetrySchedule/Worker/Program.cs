using Bus;
using Bus.RabbitMq;
using Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddRabbitMqBus()
                .BuildServiceProvider();

            var dispatchedEventBus = serviceProvider.GetService<IDispatchedEventBus>();
            
                dispatchedEventBus.CreateQueueTopology("order-created")
                .SubscribeAsync<OrderCreated>();
        }
    }
}