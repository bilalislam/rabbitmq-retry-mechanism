﻿using System;

namespace Domain
{
    public class Exchange
    {
        public string ExchangeName { get; private set; }
        public string ExchangeType { get; private set; }

        /// <summary>
        /// invariants
        /// </summary>
        /// <param name="exchangeName"></param>
        /// <param name="exchangeType"></param>
        private Exchange(string exchangeName, string exchangeType)
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
        public static Exchange Load(string exchangeName, string exchangeType)
        {
            if (string.IsNullOrEmpty(exchangeName))
                throw new Exception("exchange name should not be empty");

            if (string.IsNullOrEmpty(exchangeType))
                throw new Exception("exchange type should not be empty");

            return new Exchange(exchangeName, exchangeType);
        }
    }
}