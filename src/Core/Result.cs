using System.Collections.Generic;
using System.Linq;

namespace X.Core {
    public class Result {
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
        public string[] Errors { get; protected set; } = { };

        #region Helpers

        public static Result Success() => new() { Succeeded = true };

        public static Result Failure() => new() { Succeeded = false };

        public static Result Failure(IEnumerable<string> errors)
            => new() { Succeeded = false, Errors = errors is string[] e ? e : errors.ToArray() };

        public static Result Failure(params string[] message)
            => new() { Succeeded = false, Errors = message };

        #endregion Helpers
    }
}