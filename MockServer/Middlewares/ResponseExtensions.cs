using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MockServer.Middlewares
{
    public static class ResponseExtensions
    {
        public static async Task WriteContent(this Stream stream, string jsonContent)
        {
            var sw = new StreamWriter(stream);
            await sw.WriteAsync(jsonContent);
            await sw.FlushAsync();
            if (stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);
        }

        public static async Task WriteContent(this Stream stream, JObject jObject)
        {
            await WriteContent(stream, JsonConvert.SerializeObject(jObject));
        }

        public static async Task WriteContent(this Stream stream, object obj)
        {
            await WriteContent(stream, JsonConvert.SerializeObject(obj));
        }
    }
}
