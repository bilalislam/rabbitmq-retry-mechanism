using System.Collections.Generic;
using RabbitMQ.Client;

namespace Bus.RabbitMq
{
    public class RabbitMqHelper
    {
        public static MessageHeader DeserializeMessageHeader(IBasicProperties props)
        {
            if (props.Headers == null)
                props.Headers = new Dictionary<string, object>();

            var header = new MessageHeader();
            if (props.Headers.ContainsKey("x-retry-count"))
            {
                header.RetryCount = int.Parse(props.Headers["x-retry-count"].ToString());
            }

            return header;
        }
    }

    public class MessageHeader
    {
        public int RetryCount { get; set; }
    }
    
    public static class DictionaryExtensions
    {
        public static void AddOrUpdate(this IDictionary<string, object> headers, string key, object value)
        {
            if (headers == null)
                headers = new Dictionary<string, object>();

            if (headers.ContainsKey(key))
                headers[key] = value;
            else
                headers.Add(key, value);
        }
    } 
}