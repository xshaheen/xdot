using System;
using System.Collections.Generic;

namespace X.Domain {
    public interface IEntity<out TId> {
        TId Id { get; }
    }

    public abstract class Entity<TId> : Base<Entity<TId>>, IEntity<TId> {
        public TId Id { get; protected init; } = default!;

        protected sealed override IEnumerable<object?> Equals() { yield return Id; }
    }

    public interface IAuditable {
        DateTime CreatedAt { get; }
        DateTime? LastModifiedAt { get; }
        string? LastModifiedById { get; }
    }

    public interface IAuditable<out TUser> : IAuditable {
        TUser? LastModifiedBy { get; }
    }

    public interface ICreatorAudit {
        string CreatedById { get; }
    }

    public interface ICreatorAudit<out TUser> : ICreatorAudit {
        TUser CreatedBy { get; }
    }
}
