using System;

namespace X.Core {
	public sealed class NullDisposable : IDisposable {
		private NullDisposable() { }

		public static NullDisposable Instance { get; } = new();

		public void Dispose() { }
	}
}
