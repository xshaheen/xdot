using System;
using System.Collections.Generic;

namespace X.Domain {
    public interface IHasDomainEvent {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    }

    public abstract class DomainEvent {
        public DateTimeOffset At { get; protected set; } = DateTimeOffset.UtcNow;
    }
}
