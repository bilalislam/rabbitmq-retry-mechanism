using System;
using System.Diagnostics.CodeAnalysis;

namespace Domain
{
    [ExcludeFromCodeCoverage]
    public class OrderCreated : IEvent
    {
        public Guid EventId { get; }
        public int EventVersion { get; }
        public DateTime OccurredOn { get; }
        public Guid CorrelationId { get; set; }
    }
}