using System;

namespace Domain
{
    public interface IEvent
    {
        Guid EventId { get; }
        int EventVersion { get; }
        DateTime OccurredOn { get; }
        Guid CorrelationId { get; set; }
    }
}