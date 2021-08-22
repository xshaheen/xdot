using System;

namespace X.Domain {
    public interface IAuditable<TId> where TId : IEquatable<TId> {
        DateTimeOffset CreatedAt { get; init; }
        DateTimeOffset? LastModifiedAt { get; }
        TId? LastModifiedById { get; }
        void SetLastModified(TId by);
    }

    public interface IAuditable<TId, out TUser> : IAuditable<TId> where TId : IEquatable<TId> where TUser : class {
        TUser? LastModifiedBy { get; }
    }
}
