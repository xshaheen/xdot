using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace X.Domain.Identity
{
    public abstract class IdentityUserBase : IdentityUser, IEquatable<IdentityUserBase>, IAuditable
    {
        public DateTime CreatedAt { get; protected set; }
        public string? CreatedBy { get; protected set; }
        public DateTime? LastModifiedAt { get; private set; }
        public string? LastModifiedBy { get; private set; }
        public DateTime LastSeen { get; set; }

        public void SetCreatedAtBy(DateTime at, string by) => (CreatedAt, CreatedBy) = (at, by);

        public void UpdateLasModifiedAtBy(DateTime at, string by) => (LastModifiedAt, LastModifiedBy) = (at, by);

        public static bool operator ==(IdentityUserBase? left, IdentityUserBase? right)
        {
            if (left is null ^ right is null) return false;
            return left?.Equals(right) != false;
        }

        public static bool operator !=(IdentityUserBase left, IdentityUserBase right) => !(left == right);

        public sealed override bool Equals(object? obj) => Equals(obj as IdentityUserBase);

        public bool Equals(IdentityUserBase? other)
        {
            if (other is null) return false;

            if (ReferenceEquals(this, other)) return true;

            if (GetType() != other.GetType()) return false;

            return Equals().SequenceEqual(other.Equals());
        }

        public override int GetHashCode() => Equals().Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);

        private IEnumerable<object?> Equals() { yield return Id; }
    }
}
