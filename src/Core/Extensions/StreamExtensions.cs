using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

#pragma warning disable IDE0130
namespace System.IO {
#pragma warning restore IDE0130
    [PublicAPI]
    public static class StreamExtensions {
        public static byte[] GetAllBytes(this Stream stream) {
            using var memoryStream = new MemoryStream();
            stream.Position = 0;
            stream.CopyTo(memoryStream);

            return memoryStream.ToArray();
        }

        public static async Task<byte[]> GetAllBytesAsync(
            this Stream stream,
            CancellationToken cancellationToken = default
        ) {
            await using var memoryStream = new MemoryStream();
            stream.Position = 0;
            await stream.CopyToAsync(memoryStream, cancellationToken);

            return memoryStream.ToArray();
        }
    }
}
