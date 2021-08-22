using System.IO;
using System.IO.Compression;
using JetBrains.Annotations;

namespace X.Core.Extensions {
	[PublicAPI]
	public static class ByteExtensions {
		public static byte[] Compress(this byte[] bytes) {
			using var output = new MemoryStream();
			using var stream = new BrotliStream(output, CompressionMode.Compress);
			stream.Write(bytes, 0, bytes.Length);

			return output.ToArray();
		}

		public static byte[] Decompress(this byte[] bytes) {
			using var output = new MemoryStream();
			using var input = new MemoryStream(bytes);
			using var stream = new BrotliStream(input, CompressionMode.Decompress);
			stream.CopyTo(output);

			return output.ToArray();
		}

		/// <summary>
		/// Compares two byte[] arrays, element by element, and returns the number of elements common to both arrays.
		/// </summary>
		/// <param name="bytes1">The first byte[] to compare.</param>
		/// <param name="bytes2">The second byte[] to compare.</param>
		/// <returns>The number of common elements.</returns>
		public static int BytesDifference(this byte[] bytes1, byte[] bytes2) {
			var len1 = bytes1.Length;
			var len2 = bytes2.Length;
			var len = len1 < len2 ? len1 : len2;

			for (var i = 0; i < len; i++) {
				if (bytes1[i] != bytes2[i]) {
					return i;
				}
			}

			return len;
		}
	}
}
