using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace X.Core.Extensions {
	public static partial class StringExtensions {
		/// <summary>
		/// Limits string length to a specified value by discarding any trailing characters after the specified length.
		/// </summary>
		/// <param name="input">The <see cref="string" /> value to limit to a specified length.</param>
		/// <param name="maxLength">The maximum length allowed for <paramref name="input"/>.</param>
		/// <returns>
		/// The <paramref name="input" /> string if its length is lesser or equal than
		/// <paramref name="maxLength"/>; otherwise first <c>n</c> characters of the <paramref name="input" />,
		/// where <c>n</c> is equal to <paramref name="maxLength"/>.
		/// </returns>
		[Pure]
		[return: NotNullIfNotNull("input")]
		public static string? TruncateEnd(this string? input, [NonNegativeValue] int maxLength) {
			if (input is null)
				return null;

			return input.Length <= maxLength ? input : input[..maxLength];
		}

		/// <summary>
		/// Limits string length to a specified value by discarding a number of trailing characters and adds a specified
		/// suffix (ex "...") if any characters were discarded.
		/// </summary>
		/// <param name="input">The <see cref="string" /> value to limit to a specified length.</param>
		/// <param name="maxLength">The maximum length allowed for <paramref name="input"/>.</param>
		/// <param name="suffix">The suffix added to the result if any characters are discarded.</param>
		/// <returns>
		/// The <paramref name="input" /> string if its length is lesser or equal than
		/// <paramref name="maxLength"/>; otherwise first <c>n</c> characters of the <paramref name="input" />
		/// followed by <paramref name="suffix" />, where <c>n</c> is equal to <paramref name="maxLength"/> - length of
		/// <paramref name="suffix"/>.
		/// </returns>
		[Pure]
		[return: NotNullIfNotNull("input")]
		public static string? TruncateEnd(this string? input, [NonNegativeValue] int maxLength, string suffix) {
			if (input == null)
				return null;

			if (input == string.Empty || maxLength == 0)
				return string.Empty;

			if (input.Length <= maxLength)
				return input;

			if (maxLength <= suffix.Length)
				return suffix[..maxLength];

			return input[..(maxLength - suffix.Length)] + suffix;
		}

		/// <summary>
		/// Limits string length to a specified value by discarding any starting characters before the specified length.
		/// </summary>
		/// <param name="input">The <see cref="string" /> value to limit to a specified length.</param>
		/// <param name="maxLength">The maximum length allowed for <paramref name="input"/>.</param>
		/// <returns>
		/// The <paramref name="input" /> string if its length is lesser or equal than
		/// <paramref name="maxLength"/>; otherwise last <c>n</c> characters of the <paramref name="input" />,
		/// where <c>n</c> is equal to <paramref name="maxLength"/>.
		/// </returns>
		[Pure]
		[return: NotNullIfNotNull("input")]
		public static string? TruncateStart(this string? input, [NonNegativeValue] int maxLength) {
			if (input is null)
				return null;

			return input.Length <= maxLength
				? input
				: input.Substring(input.Length - maxLength, maxLength);
		}
	}
}
