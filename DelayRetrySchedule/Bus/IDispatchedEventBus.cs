using System.Threading.Tasks;
using Domain;
using RabbitMQ.Client;

namespace Bus
{
    public interface IDispatchedEventBus
    {
        IDispatchedEventBus CreateQueueTopology(string queueName);

        Task<bool> PublishMessageAsync<TMessage>(TMessage msg, string exchangeName, string exchangeType,
            string routingKey, IBasicProperties basicProperties = null) where TMessage : IEvent;

        Task SubscribeAsync<TMessage>() where TMessage : IEvent;
    }
}