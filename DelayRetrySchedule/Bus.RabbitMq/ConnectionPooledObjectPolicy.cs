using System;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace Bus.RabbitMq
{
    public class ConnectionPooledObjectPolicy : IPooledObjectPolicy<IConnection>
    {
        public IConnection Create()
        {
            var factory = new ConnectionFactory
            {
                HostName = "10.10.1.101",
                UserName = "alfred",
                Password = "B@tmanSavesTheW0rld!",
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
                UseBackgroundThreadsForIO = false
            };
            return factory.CreateConnection();
        }

        public bool Return(IConnection obj)
        {
            return true;
        }
    }
}