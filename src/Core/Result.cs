using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace X.Core {
    [PublicAPI]
    public class Result<TError> {
        private Result() { }

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// Flag indicating whether if the operation failed or not.
        /// </summary>
        /// <value>True if the operation failed, otherwise true.</value>
        public bool Failed => !Succeeded;

        /// <summary>
        /// Collection of errors messages when fails. Will be null if operation succeeded.
        /// </summary>
        public TError[] Errors { get; protected set; } = Array.Empty<TError>();

        #region Helpers

        public static Result<TError> Success() {
            return new() { Succeeded = true };
        }

        public static Result<TError> Failure() {
            return new() { Succeeded = false };
        }

        public static Result<TError> Failure(IEnumerable<TError> errors) {
            return new() {
                Succeeded = false,
                Errors    = errors is TError[] e ? e : errors.ToArray(),
            };
        }

        public static Result<TError> Failure(params TError[] message) {
            return new() { Succeeded = false, Errors = message };
        }

        #endregion Helpers
    }
}
