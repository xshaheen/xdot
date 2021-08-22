using System.Collections.Generic;

namespace X.Domain {
    public interface IHasDomainEvent {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    }
}
