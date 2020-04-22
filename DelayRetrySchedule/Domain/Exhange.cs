using System;

namespace Domain
{
    public class Exhange
    {
        public string ExchangeName { get; private set; }
        public string ExchangeType { get; private set; }

        /// <summary>
        /// invariants
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="exchangeType"></param>
        private Exhange(string exchangeName, string exchangeType)
        {
            ExchangeName = exchangeName;
            ExchangeType = exchangeType;
        }

        /// <summary>
        /// guard pattern
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Exhange Load(string exchangeName, string exchangeType)
        {
            if (string.IsNullOrEmpty(exchangeName))
                throw new Exception("exchange name should not be empty");

            if (string.IsNullOrEmpty(exchangeType))
                throw new Exception("exchange type should not be empty");

            return new Exhange(exchangeName, exchangeType);
        }
    }
}