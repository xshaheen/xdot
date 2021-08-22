using System;
using JetBrains.Annotations;

namespace X.Core {
	[PublicAPI]
	public class Result<TError> {
		protected Result() { }

		/// <summary>Flag indicating whether if the operation succeeded or not.</summary>
		/// <value>True if the operation succeeded, otherwise false.</value>
		public bool Succeeded { get; protected set; }

		/// <summary>Flag indicating whether if the operation failed or not.</summary>
		/// <value>True if the operation failed, otherwise true.</value>
		public bool Failed => !Succeeded;

		/// <summary>Collection of errors messages when fails. Will be null if operation succeeded.</summary>
		public TError[] Errors { get; protected set; } = Array.Empty<TError>();
	}
}
