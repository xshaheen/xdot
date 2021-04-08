using System.Diagnostics.CodeAnalysis;
using System.Text;
using JetBrains.Annotations;

namespace X.Core.Utils {
    [PublicAPI]
    public class StringHelper {
        public static Encoding Utf8WithoutBom { get; } = new UTF8Encoding(false);

        /// <summary>
        /// Converts a byte[] to string without BOM (byte order mark).
        /// </summary>
        /// <param name="bytes">The byte[] to be converted to string</param>
        /// <param name="encoding">The encoding to get string. Default is UTF8</param>
        /// <returns></returns>
        [return: NotNullIfNotNull("bytes")]
        public static string? ConvertFromBytesWithoutBom(byte[]? bytes, Encoding? encoding = null) {
            if (bytes == null) {
                return null;
            }

            encoding ??= Encoding.UTF8;

            var hasBom = bytes.Length >= 3
                && bytes[0] == 0xEF
                && bytes[1] == 0xBB
                && bytes[2] == 0xBF;

            return hasBom
                ? encoding.GetString(bytes, 3, bytes.Length - 3)
                : encoding.GetString(bytes);
        }
    }
}
