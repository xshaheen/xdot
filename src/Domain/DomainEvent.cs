using System;
using System.Collections.Generic;

namespace X.Domain {
    public interface IHasDomainEvent {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    }

    public abstract class DomainEvent {
        protected DomainEvent() {
            Id = Guid.NewGuid();
            At = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; protected set; }

        public DateTimeOffset At { get; protected set; }

        public bool IsPublished { get; set; }
    }
}
