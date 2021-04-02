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

    public interface IAuditable<TId> where TId : IEquatable<TId> {
        DateTime CreatedAt { get; init; }
        DateTime? LastModifiedAt { get; }
        TId? LastModifiedById { get; }
        void SetLastModified(TId by);
    }

    public interface IAuditable<TId, out TUser> : IAuditable<TId>
        where TId : IEquatable<TId>
        where TUser : class {
        TUser? LastModifiedBy { get; }
    }

    public interface ICreatorAudit<TId> where TId : IEquatable<TId> {
        TId CreatedById { get; set; }
    }

    public interface ICreatorAudit<TId, out TUser> : ICreatorAudit<TId>
        where TId : IEquatable<TId>
        where TUser : class {
        TUser CreatedBy { get; }
    }
}
