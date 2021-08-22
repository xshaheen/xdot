using JetBrains.Annotations;
using X.Core.Utils;

namespace X.Core.Extensions {
	/// <summary>Arabic char extensions.</summary>
	public static class ArCharExtensions {
		/// <summary>Checks for Arabic Shadda Mark.</summary>
		/// <param name="c">arabic unicode char</param>
		[Pure]
		public static bool IsShadda(this char c) {
			return c == Ar.Shadda;
		}

		/// <summary>Checks for Arabic Tatweel letter modifier.</summary>
		/// <param name="c">arabic unicode char</param>
		[Pure]
		public static bool IsTatweel(this char c) {
			return c == Ar.Tatweel;
		}

		/// <summary>Checks for Arabic Tatweel letter modifier.</summary>
		/// <param name="c">arabic unicode char</param>
		[Pure]
		public static bool IsTanwin(this char c) {
			return c.IsIn(Ar.Tanwin);
		}

		/// <summary>Checks for Arabic Tashkeel.</summary>
		/// <param name="c">arabic unicode char</param>
		[Pure]
		public static bool IsTashkeel(this char c) {
			return c.IsIn(Ar.Tashkeel);
		}

		/// <summary>Checks for Arabic moon letters.</summary>
		/// <param name="c">arabic unicode char</param>
		[Pure]
		public static bool IsMoon(this char c) {
			return c.IsIn(Ar.Moon);
		}

		/// <summary>Checks for Arabic sun letters.</summary>
		/// <param name="c">arabic unicode char</param>
		[Pure]
		public static bool IsSun(this char c) {
			return c.IsIn(Ar.Sun);
		}

		/// <summary>Checks for Arabic hamza like letter.</summary>
		/// <param name="c">arabic unicode char</param>
		[Pure]
		public static bool IsHamza(this char c) {
			return c.IsIn(Ar.Hamzat);
		}

		/// <summary>Checks for Arabic alef like letter.</summary>
		/// <param name="c">arabic unicode char</param>
		[Pure]
		public static bool IsAlef(this char c) {
			return c.IsIn(Ar.Alefat);
		}

		/// <summary>Checks for Arabic yeh like letter.</summary>
		/// <param name="c">arabic unicode char</param>
		[Pure]
		public static bool IsYehLike(this char c) {
			return c.IsIn(Ar.YehLike);
		}

		/// <summary>Checks for Arabic waw like letter.</summary>
		/// <param name="c">arabic unicode char</param>
		[Pure]
		public static bool IsWawLike(this char c) {
			return c.IsIn(Ar.WawLike);
		}
	}
}
