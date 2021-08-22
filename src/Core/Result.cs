using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace X.Core {
	[PublicAPI]
	public class Result : Result<ErrorDescriptor> {
		public static Result Success() {
			return new Result { Succeeded = true };
		}

		public static Result Failure() {
			return new Result { Succeeded = false };
		}

		public static Result Failure(IEnumerable<ErrorDescriptor> errors) {
			return new Result {
				Succeeded = false,
				Errors = errors is ErrorDescriptor[] e ? e : errors.ToArray(),
			};
		}

		public static Result Failure(params ErrorDescriptor[] message) {
			return new Result { Succeeded = false, Errors = message };
		}
	}
}
