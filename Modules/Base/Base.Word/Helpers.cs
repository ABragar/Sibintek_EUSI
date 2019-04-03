using System.IO;
using System.Threading.Tasks;

namespace Base.Word
{
    public class Helpers
    {

        public static async Task<byte[]> ReadBytesAsync(string path)
        {
            using (var file = File.OpenRead(path))
            {
                var remaining = (int)file.Length;

                var buffer = new byte[remaining];

                var offset = 0;
                int readed;
                do
                {

                    readed = await file.ReadAsync(buffer, offset, remaining);
                    offset += readed;
                    remaining -= readed;

                } while (readed > 0);

                return buffer;

            }
        }


        public static async Task<MemoryStream> CopyToMemoryAsync(byte[] buffer)
        {
            var stream = new MemoryStream();
            try
            {
                await stream.WriteAsync(buffer, 0, buffer.Length);

                return stream;
            }
            catch
            {
                stream.Dispose();
                throw;
            }

        }

        public static async Task<MemoryStream> ReadToMemoryAsync(string path)
        {
            var stream = new MemoryStream();
            try
            {
                using (var file = File.OpenRead(path))
                {
                    await file.CopyToAsync(stream);
                    return stream;
                }
            }
            catch
            {
                stream.Dispose();
                throw;
            }

        }
    }
}