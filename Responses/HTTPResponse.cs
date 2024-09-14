using System.Text;

namespace HttpServer
{
    public class HTTPResponse
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();
        public string Body { get; set; }

        public HTTPResponse(int statusCode = 200, string statusMessage = "OK", string body = "")
        {
            StatusCode = statusCode;
            StatusMessage = statusMessage;
            Body = body;
        }

        public override string ToString()
        {
            StringBuilder response = new StringBuilder();
            response.AppendLine($"HTTP/1.1 {StatusCode} {StatusMessage}");
            foreach (var header in Headers)
            {
                response.AppendLine($"{header.Key}: {header.Value}");
            }
            response.AppendLine($"Content-Length: {Encoding.UTF8.GetByteCount(Body)}\r\n");
            response.AppendLine(Body);
            return response.ToString();
        }
    }
}
