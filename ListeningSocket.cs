namespace HttpServer
{
    using System.Net.Sockets;
    using System.Net;
    using System.Text;
    using System.Collections.Generic;

    public class ListeningSocket
    {
        private string ip;
        private int port;
        private Router router;
        private ApiController controllerInstance;

        public ListeningSocket(string ip = "127.0.0.1", int port = 57157)
        {
            this.ip = ip;
            this.port = port;
            router = new Router(typeof(ApiController));
            controllerInstance = new ApiController();
        }

        public void StartListeningSocket()
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            TcpListener listener = new TcpListener(ipAddress, port);

            Console.WriteLine("Listening on {0}:{1}", ip, port);

            listener.Start();

            while (true)
            {
                Socket client = listener.AcceptSocket();

                var childSocketThread = new Thread(() =>
                {
                    List<string> requestLines = new List<string>();
                    StringBuilder requestData = new StringBuilder();
                    byte[] dataBuffer = new byte[1024];
                    int receivedBytes;

                    while ((receivedBytes = client.Receive(dataBuffer)) > 0)
                    {
                        string chunk = Encoding.UTF8.GetString(dataBuffer, 0, receivedBytes);

                        requestData.Append(chunk);

                        if (requestData.ToString().Contains("\r\n\r\n"))
                        {
                            break;
                        }
                    }

                    string[] lines = requestData.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                    requestLines.AddRange(lines);

                    HTTPRequest httpRequest = new HTTPRequest(requestLines);

                    HTTPResponse httpResponse = router.RouteRequest(httpRequest.Method, httpRequest.Path, httpRequest, controllerInstance);

                    byte[] responseBytes = Encoding.UTF8.GetBytes(httpResponse.ToString());
                    client.Send(responseBytes);

                    client.Close();
                });

                childSocketThread.Start();
            }
        }
    }
}
