using System.Collections.Generic;

namespace X.Domain {
    public interface IEntity<out TId> {
        TId Id { get; }
    }

    public abstract class Entity<TId> : Base<Entity<TId>>, IEntity<TId> {
        public TId Id { get; protected init; } = default!;

        protected sealed override IEnumerable<object?> Equals() { yield return Id; }
    }
}
