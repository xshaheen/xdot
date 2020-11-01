using System;
using System.Collections.Generic;

namespace X.Domain
{
    public abstract class Entity<TId> : Base<Entity<TId>>
    {
        public TId Id { get; protected set; } = default!;

        protected sealed override IEnumerable<object?> Equals() { yield return Id; }
    }

    public interface IAuditable
    {
        DateTime CreatedAt { get; }
        string? CreatedBy { get; }
        DateTime? LastModifiedAt { get; }
        string? LastModifiedBy { get; }

        void UpdateLasModifiedAtBy(DateTime at, string by);

        void SetCreatedAtBy(DateTime at, string by);
    }

    public abstract class Auditable : IAuditable
    {
        public DateTime CreatedAt { get; protected set; }
        public string? CreatedBy { get; protected set; }
        public DateTime? LastModifiedAt { get; protected set; }
        public string? LastModifiedBy { get; protected set; }

        public void UpdateLasModifiedAtBy(DateTime at, string by) { (LastModifiedAt, LastModifiedBy) = (at, by); }

        public void SetCreatedAtBy(DateTime at, string by) { (CreatedAt, CreatedBy) = (at, by); }
    }

    public abstract class AuditableEntity<TId> : Entity<TId>, IAuditable
    {
        public DateTime CreatedAt { get; protected set; }
        public string? CreatedBy { get; protected set; }
        public DateTime? LastModifiedAt { get; protected set; }
        public string? LastModifiedBy { get; protected set; }

        public void UpdateLasModifiedAtBy(DateTime at, string by) { (LastModifiedAt, LastModifiedBy) = (at, by); }

        public void SetCreatedAtBy(DateTime at, string by) { (CreatedAt, CreatedBy) = (at, by); }
    }
}
