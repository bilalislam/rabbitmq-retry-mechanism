using System;
using Domain.Dtos;

namespace Domain
{
    public class Queue
    {
        public string QueueName { get; private set; }

        //Bounded Context
        public Exhange Exhange { get; private set; }

        public string RetryExchange => GetRetryExchangeName();
        public string DelayQueue => GetDelayQueueName();
        public string RetryQueue => GetRetryQueueName();


        //Model driven design practice
        private string GetRetryExchangeName()
        {
            return $"{Exhange.ExchangeName}.retry";
        }

        private string GetDelayQueueName()
        {
            return $"{QueueName}.delay";
        }

        private string GetRetryQueueName()
        {
            return $"{QueueName}.retry";
        }

        private Queue(string queuue, Exhange exhange)
        {
            QueueName = queuue;
            Exhange = exhange;
        }

        /// <summary>
        /// queue invariants
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="useScheduler"></param>
        /// <param name="exhange"></param>
        /// <returns></returns>
        public static Queue Load(string queueName, ExchangeDto exhange)
        {
            if (string.IsNullOrEmpty(queueName))
                throw new Exception("queue name should not be empty");

            if (exhange == null)
                throw new Exception("exchange should not be null");

            return new Queue(
                queueName,
                Exhange.Load(exhange.ExhangeName, exhange.ExchangeType));
        }
    }
}