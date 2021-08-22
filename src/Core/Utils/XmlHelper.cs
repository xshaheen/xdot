using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using JetBrains.Annotations;
using X.Core.Extensions;

namespace X.Core.Utils {
	[PublicAPI]
	public class XmlHelper {
		/// <summary>Remove hidden characters then XML Encode</summary>
		public static async Task<string?> XmlEncodeAsync(string? str, XmlWriterSettings? settings = null) {
			return str is not null
				? await XmlEncodeAsIsAsync(str.RemoveHiddenChars(), settings)
				: null;
		}

		/// <summary>XML Encode as is</summary>
		public static async Task<string?> XmlEncodeAsIsAsync(string? str, XmlWriterSettings? settings = null) {
			if (str == null)
				return null;

			settings ??= new XmlWriterSettings {
				Async = true,
				ConformanceLevel = ConformanceLevel.Auto,
			};

			await using var sw = new StringWriter();
			await using var xwr = XmlWriter.Create(sw, settings);
			await xwr.WriteStringAsync(str);
			await xwr.FlushAsync();

			return sw.ToString();
		}

		/// <summary>Decodes an attribute</summary>
		public static string XmlDecode(string input) {
			var sb = new StringBuilder(input);

			var rez = sb
				.Replace("&quot;", "\"")
				.Replace("&apos;", "'")
				.Replace("&lt;", "<")
				.Replace("&gt;", ">")
				.Replace("&amp;", "&")
				.ToString();

			return rez;
		}

		/// <summary>Serializes a datetime</summary>
		public static async Task<string> SerializeDateTimeAsync(DateTime dateTime) {
			var xmlS = new XmlSerializer(typeof(DateTime));
			var sb = new StringBuilder();
			await using var sw = new StringWriter(sb);
			xmlS.Serialize(sw, dateTime);

			return sb.ToString();
		}
	}
}
