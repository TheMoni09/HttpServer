using System.Text;

namespace HttpServer
{
    public class HTTPRequest
    {
        public string Method { get; }
        public string Path { get; }
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();
        public string Body { get; }

        public HTTPRequest(List<string> rawRequest)
        {
            if (rawRequest == null || rawRequest.Count == 0 || string.IsNullOrWhiteSpace(rawRequest[0]))
            {
                Console.WriteLine("Invalid HTTP request data: Request is empty or malformed.");
            }

            string[] requestLine = rawRequest[0].Split(' ');
            if (requestLine.Length < 2)
            {
                Console.WriteLine("Invalid HTTP request line.");
            }

            Method = requestLine[0];
            Path = requestLine[1];

            int i = 1;
            while (i < rawRequest.Count && !string.IsNullOrWhiteSpace(rawRequest[i]))
            {
                string[] headerParts = rawRequest[i].Split(new[] { ": " }, 2, StringSplitOptions.None);
                if (headerParts.Length == 2)
                {
                    Headers[headerParts[0]] = headerParts[1];
                }
                i++;
            }

            if (rawRequest.Count > i + 1)
            {
                Body = string.Join("\r\n", rawRequest.Skip(i + 1).ToArray());
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"{Method} {Path}");
            foreach (var header in Headers)
            {
                sb.AppendLine($"{header.Key}: {header.Value}");
            }

            if (!string.IsNullOrEmpty(Body))
            {
                sb.AppendLine("\n" + Body);
            }

            return sb.ToString();
        }
    }
}
