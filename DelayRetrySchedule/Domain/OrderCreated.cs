using System;

namespace Domain
{
    public class OrderCreated : IEvent
    {
        public Guid EventId { get; }
        public int EventVersion { get; }
        public DateTime OccurredOn { get; }
        public Guid CorrelationId { get; set; }
    }
}