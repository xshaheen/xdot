using System.Collections.Generic;
using System.Globalization;
using System.Text;
using JetBrains.Annotations;

namespace X.Core.Extensions {
	public static partial class StringExtensions {
		/// <summary>Convert any digit in the string to the equivalent Arabic digit [0-9].</summary>
		/// <example>"١٢٨" to "128"</example>
		/// <example>"١,٢٨" to "1,28"</example>
		/// <example>"١.٢٨" to "1.28"</example>
		/// <example>"This ١٢٨" to "This 128"</example>
		[Pure] public static string ToInvariantDigits(this string input) {
			if (string.IsNullOrWhiteSpace(input))
				return input;

			var sb = new StringBuilder();

			foreach (var c in input) {
				if (char.IsDigit(c))
					sb.Append(char.GetNumericValue(c).ToString(CultureInfo.InvariantCulture));
				else
					sb.Append(c);
			}

			return sb.ToString();
		}

		/// <summary>Convert any digit in the string to the equivalent Arabic digit [0-9].</summary>
		/// <example>"١٢٨" to "128"</example>
		/// <example>"١,٢٨" to "1,28"</example>
		/// <example>"١.٢٨" to "1.28"</example>
		/// <example>"This ١٢٨" to "This 128"</example>
		[Pure] public static string ToInvariantDigits(this IEnumerable<char> input) {
			var sb = new StringBuilder();
			foreach (var c in input) {
				if (char.IsDigit(c))
					sb.Append(char.GetNumericValue(c).ToString(CultureInfo.InvariantCulture));
				else
					sb.Append(c);
			}

			return sb.ToString();
		}
	}
}
