namespace HttpServer
{
    public class ApiController
    {
        [GetHandler("/api/test")]
        public HTTPResponse TestGetHandler(HTTPRequest request)
        {
            foreach (KeyValuePair<string, string> header in request.Headers)
            {
                Console.WriteLine("{0}: {1}", header.Key, header.Value);
            }

            var response = new HTTPResponse
            {
                StatusCode = 200,
                StatusMessage = "OK",
                Body = "<html><body><h1>GET request received at /api/test</h1></body></html>"
            };

            response.Headers.Add("Content-Type", "text/html");
            return response;
        }

        [PostHandler("/api/test")]
        public HTTPResponse TestPostHandler(HTTPRequest request)
        {
            var response = new HTTPResponse
            {
                StatusCode = 201,
                StatusMessage = "Created",
                Body = $"<html><body><h1>POST request received at /api/test</h1><p>Data received: {request.Body}</p></body></html>"
            };

            response.Headers.Add("Content-Type", "text/html");
            return response;
        }
    }
}
