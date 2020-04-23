using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Dtos;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Bus.RabbitMq
{
    public class DispatchedEventBus : IDispatchedEventBus
    {
        private string _queueName { get; set; }

        private readonly ObjectPool<IConnection> _connectionPool;

        public DispatchedEventBus(IPooledObjectPolicy<IConnection> objectPolicy){
            _connectionPool = new DefaultObjectPool<IConnection>(objectPolicy);
        }


        public Task<bool> PublishMessageAsync<TMessage>(TMessage msg, string exchangeName, string exchangeType,
            string routingKey,
            IBasicProperties basicProperties = null) where TMessage : IEvent{
            var conn = _connectionPool.Get();
            try{
                using (var model = conn.CreateModel()){
                    var body = new Serializer().Serialize(msg);
                    var props = basicProperties ?? model.CreateBasicProperties();
                    props.Persistent = true;

                    model.ExchangeDeclare(exchangeName, exchangeType, true, false, null);
                    model.BasicPublish(exchangeName, routingKey, false, props, body);

                    return Task.FromResult(true);
                }
            }
            finally{
                _connectionPool.Return(conn);
            }
        }

        public Task SubscribeAsync<TMessage>() where TMessage : IEvent{
            var model = _connectionPool.Get().CreateModel();
            var consumer = new EventingBasicConsumer(model);
            consumer.Received += (sender, args) =>
            {
                var message = new Serializer().Deserialize<TMessage>(args.Body);

                Console.WriteLine(
                    $"[BUS: {this._queueName}] {typeof(TMessage).Name} received by subscriber[DeliveryTag:{args.DeliveryTag}" +
                    $" Consumer:{args.ConsumerTag} Payload: {@message}]",
                    this._queueName,
                    typeof(TMessage).Name,
                    args.DeliveryTag,
                    args.ConsumerTag,
                    message);

                var messageHeader = RabbitMqHelper.DeserializeMessageHeader(args.BasicProperties);
                Console.WriteLine("header : " + messageHeader.RetryCount);
                if (messageHeader.RetryCount >= 100){
                    SendToDeadLetter(message, "Exchange.retry", $"{this._queueName}.retry", args.BasicProperties);
                    model.BasicAck(args.DeliveryTag, true);
                    return;
                }

                try{
                    if (messageHeader.RetryCount == 50){
                        model.BasicAck(args.DeliveryTag, true);
                    }
                    else{
                        throw new Exception();
                    }


                    Console.WriteLine(
                        $"[BUS: {this._queueName}] {typeof(TMessage).Name} processed by subscriber[DeliveryTag:{args.DeliveryTag}" +
                        $" Consumer:{args.ConsumerTag} Payload: {@message}]",
                        this._queueName,
                        typeof(TMessage).Name,
                        args.DeliveryTag,
                        args.ConsumerTag,
                        message);
                }
                catch (Exception ex) when (
                    ex is RabbitMQ.Client.Exceptions.ConnectFailureException ||
                    ex is RabbitMQ.Client.Exceptions.AlreadyClosedException){
                    Console.WriteLine(
                        $"[BUS: {this._queueName}] {typeof(TMessage).Name} subscriber[DeliveryTag:{args.DeliveryTag} " +
                        $"Consumer:{args.ConsumerTag}] Reason: {ex}",
                        new {Message = message});
                }
                catch (Exception ex){
                    Retry(message, this._queueName, ex.ToString(), messageHeader.RetryCount,
                        args.BasicProperties);
                }
            };

            consumer.Shutdown += OnConsumerShutdown;
            model.BasicConsume(this._queueName, false, consumer);
            model.BasicQos(0, 300, false);

            return Task.CompletedTask;
        }


        public IDispatchedEventBus CreateQueueTopology(string queueName){
            var queue = Queue.Load(queueName, new ExchangeDto()
            {
                ExhangeName = "Exchange",
                ExchangeType = "topic"
            });

            using (var model = _connectionPool.Get().CreateModel()){
                model.ExchangeDeclare(queue.Exhange.ExchangeName, queue.Exhange.ExchangeType, true, false, null);
                model.ExchangeDeclare(queue.RetryExchange, queue.Exhange.ExchangeType, true);

                model.QueueDeclare(queueName, true, false, false, new Dictionary<string, object>()
                {
                    {"x-dead-letter-exchange", queue.RetryExchange},
                    {"x-dead-letter-routing-key", queue.RetryQueue},
                });

                model.QueueDeclare(queue.RetryQueue, true, false, false, new Dictionary<string, object>()
                {
                    {"x-dead-letter-exchange", queue.Exhange.ExchangeName},
                    {"x-dead-letter-routing-key", queue.QueueName},
                });

                model.QueueBind(queue.QueueName, "Exchange", queue.QueueName);
                model.QueueBind(queue.RetryQueue, queue.RetryExchange, queue.RetryQueue);

                model.QueueDeclare(queue.DelayQueue, true, false, false,
                    new Dictionary<string, object>()
                    {
                        {"x-dead-letter-exchange", queue.Exhange.ExchangeName},
                        {"x-dead-letter-routing-key", queue.QueueName},
                        {"x-message-ttl", 3000}
                    });

                model.QueueBind(queue.DelayQueue, "Exchange", queue.DelayQueue);
            }

            this._queueName = queue.QueueName;
            return this;
        }

        private void SendToDeadLetter<TMessage>(TMessage message, string exc, string rq,
            IBasicProperties props) where TMessage : IEvent{
            PublishMessageAsync(message, exc, "topic", exc, props);
        }

        private void Retry<TMessage>(TMessage message, string queueName, string deadReason, int attempt,
            IBasicProperties props) where TMessage : IEvent{
            props.Headers.AddOrUpdate("x-dead-reason", deadReason);
            props.Headers.AddOrUpdate("x-retry-count", attempt + 1);

            string retryQueue = $"{queueName}.delay";
            PublishMessageAsync(message, "Exchange", "topic",
                retryQueue, props);
        }

        private void OnConsumerShutdown(object sender, ShutdownEventArgs e){
            Console.WriteLine($"Consumer has been down ReplyText: {e.ReplyText} ReplyCode: {e.ReplyCode}");
        }
    }
}