using System;
using System.Collections.Generic;
using System.Linq;

namespace X.Domain {
	public abstract class Base<T> : IEquatable<Base<T>> {
		public bool Equals(Base<T>? other) {
			if (other is null)
				return false;

			if (ReferenceEquals(this, other))
				return true;

			return GetType() == other.GetType() && Equals().SequenceEqual(other.Equals());
		}

		public static bool operator ==(Base<T>? left, Base<T>? right) {
			if (left is null ^ right is null)
				return false;

			return left?.Equals(right) != false;
		}

		public static bool operator !=(Base<T> left, Base<T> right) {
			return !(left == right);
		}

		public sealed override bool Equals(object? obj) {
			return Equals(obj as Base<T>);
		}

		public override int GetHashCode() {
			return Equals().Select(x => x != null ? x.GetHashCode() : 0)
				.Aggregate((x, y) => x ^ y);
		}

		protected abstract IEnumerable<object?> Equals();
	}
}
