using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace X.Core {
    [PublicAPI]
    public class Result<TError> {
        protected Result() { }

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
    }

    [PublicAPI]
    public class Result : Result<ErrorDescriptor> {
        public static Result Success() {
            return new() { Succeeded = true };
        }

        public static Result Failure() {
            return new() { Succeeded = false };
        }

        public static Result Failure(IEnumerable<ErrorDescriptor> errors) {
            return new() {
                Succeeded = false,
                Errors = errors is ErrorDescriptor[] e ? e : errors.ToArray(),
            };
        }

        public static Result Failure(params ErrorDescriptor[] message) {
            return new() { Succeeded = false, Errors = message };
        }
    }
}
