using System.Diagnostics.CodeAnalysis;

namespace Domain.Dtos
{
    [ExcludeFromCodeCoverage]
    public class QueueDto
    {
        public string QueueName { get; set; }
        public ExchangeDto Exchange { get; set; }
    }
}