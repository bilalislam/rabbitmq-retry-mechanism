using System.IO;
using Newtonsoft.Json;

namespace Bus.RabbitMq
{
    public class Serializer
    {
        public byte[] Serialize<T>(T value)
        {
            using (var ms = new MemoryStream())
            {
                using (var sr = new StreamWriter(ms))
                using (var jtr = new JsonTextWriter(sr))
                {
                    new JsonSerializer().Serialize(jtr, value);
                }

                return ms.ToArray();
            }
        }

        public T Deserialize<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            using (var sr = new StreamReader(ms))
            using (var jtr = new JsonTextReader(sr))
            {
                return new JsonSerializer().Deserialize<T>(jtr);
            }
        }
    }
}