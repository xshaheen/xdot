using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace X.Domain.Identity {
    public abstract class IdentityUserBase<TModifier> : IdentityUser, IEntity<string>,
        IEquatable<IdentityUserBase<TModifier>>, IAuditable<TModifier> {
        public DateTime LastSeen { get; set; }
        public DateTime CreatedAt { get; protected set; }
        public DateTime? LastModifiedAt { get; protected set; }
        public string? LastModifiedById { get; protected set; }
        public TModifier? LastModifiedBy { get; protected init; }

        public void SetLasModification(DateTime at, string by)
            => (LastModifiedAt, LastModifiedById) = (at, by);

        public bool Equals(IdentityUserBase<TModifier>? other) {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            return Equals().SequenceEqual(other.Equals());
        }

        public static bool operator==(
            IdentityUserBase<TModifier>? left,
            IdentityUserBase<TModifier>? right
        ) {
            if (left is null ^ right is null)
                return false;
            return left?.Equals(right) != false;
        }

        public static bool operator!=(
            IdentityUserBase<TModifier> left,
            IdentityUserBase<TModifier> right
        ) => !(left == right);

        public sealed override bool Equals(object? obj)
            => Equals(obj as IdentityUserBase<TModifier>);

        public override int GetHashCode() => Equals().Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);

        private IEnumerable<object?> Equals() { yield return Id; }
    }
}