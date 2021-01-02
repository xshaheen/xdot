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
        TUser LastModifiedBy { get; }
    }

    public interface ICreatorAudit {
        string CreatedById { get; }
    }

    public interface ICreatorAudit<out TUser> : ICreatorAudit {
        TUser CreatedBy { get; }
    }

    public abstract class AuditableEntity<TId, TUser>
        : IEntity<TId>, IAuditable<TUser>, ICreatorAudit<TUser> {
        public DateTime CreatedAt { get; protected set; }

        public DateTime? LastModifiedAt { get; protected set; }
        public string? LastModifiedById { get; protected set; } = default!;
        public TUser LastModifiedBy { get; protected init; } = default!;
        public string CreatedById { get; protected set; } = default!;
        public TUser CreatedBy { get; protected init; } = default!;
        public TId Id { get; protected set; } = default!;
    }
}