using System;
using System.Collections.Generic;

namespace X.Domain {
    public interface IEntity<out TId> {
        TId Id { get; }
    }

    public interface IEntity : IEntity<string> { }

    public abstract class Entity<TId> : Base<Entity<TId>>, IEntity<TId> {
        public TId Id { get; protected init; } = default!;

        protected sealed override IEnumerable<object?> Equals() { yield return Id; }
    }

    public abstract class Entity : Base<Entity>, IEntity<string> {
        public string Id { get; protected init; } = default!;

        protected sealed override IEnumerable<object?> Equals() { yield return Id; }
    }



    public interface ICreatorAudit<TId> where TId : IEquatable<TId> {
        TId CreatedById { get; set; }
    }

    public interface ICreatorAudit : ICreatorAudit<string> { }

    public interface ICreatorAudit<TId, out TUser> : ICreatorAudit<TId>
        where TId : IEquatable<TId>
        where TUser : class {
        TUser CreatedBy { get; }
    }
}
