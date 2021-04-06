using System;

namespace X.Domain {
    public interface IAuditable<TId> where TId : IEquatable<TId> {
        DateTime CreatedAt { get; init; }
        DateTime? LastModifiedAt { get; }
        TId? LastModifiedById { get; }
        void SetLastModified(TId by);
    }

    public interface IAuditable : IAuditable<string> { }

    public interface IAuditable<TId, out TUser> : IAuditable<TId>
        where TId : IEquatable<TId>
        where TUser : class {
        TUser? LastModifiedBy { get; }
    }
}
