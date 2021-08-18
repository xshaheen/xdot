using System;
using System.Collections.Generic;
using MediatR;

namespace X.Domain {
    public interface IHasDomainEvent {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    }

    public abstract class DomainEvent : INotification {
        protected DomainEvent() {
            Id = Guid.NewGuid();
            At = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; protected set; }

        public DateTimeOffset At { get; protected set; }

        public bool IsPublished { get; set; }
    }
}
