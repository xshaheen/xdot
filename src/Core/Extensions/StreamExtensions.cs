using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace X.Core.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] GetAllBytes(this Stream stream)
        {
            using var memoryStream = new MemoryStream();

            stream.Position = 0;
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public static async Task<byte[]> GetAllBytesAsync(
            this Stream stream,
            CancellationToken cancellationToken = default)
        {
            await using var memoryStream = new MemoryStream();

            stream.Position = 0;
            await stream.CopyToAsync(memoryStream, cancellationToken);
            return memoryStream.ToArray();
        }

        public static Task CopyToAsync(
            this Stream stream,
            Stream destination,
            CancellationToken cancellationToken = default)
        {
            stream.Position = 0;
            return stream.CopyToAsync(
                destination,
                81920, //this is already the default value, but needed to set to be able to pass the cancellationToken
                cancellationToken
            );
        }
    }
}
