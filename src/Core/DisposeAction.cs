using System;
using Ardalis.GuardClauses;
using JetBrains.Annotations;

namespace X.Core {
	/// <summary>This class can be used to provide an action when Dispose method is called.</summary>
	[PublicAPI]
	public class DisposeAction : IDisposable {
		private readonly Action _action;

		/// <summary>Creates a new <see cref="DisposeAction"/> object.</summary>
		/// <param name="action">Action to be executed when this object is disposed.</param>
		public DisposeAction(Action action) {
			_action = Guard.Against.Null(action, nameof(action));
		}

		public void Dispose() {
			_action();
			GC.SuppressFinalize(this);
		}
	}
}
