using System.Net.Sockets;
using System.Net;
using System.Text;

public class ListeningSocket
{
    private string ip;
    private int port;

    public ListeningSocket(string ip = "127.0.0.1", int port = 57157)
    {
        this.ip = ip;
        this.port = port;
    }

    public void StartListeningSocket()
    {
        IPAddress ipAddress = IPAddress.Parse(ip);
        TcpListener listener = new TcpListener(ipAddress, port);

        Console.WriteLine("Starting Listening TCP Server on {0}:{1}", ip, port);

        listener.Start();

        Console.WriteLine("Listening on {0}:{1}", ip, port);

        while (true)
        {
            Socket client = listener.AcceptSocket();
            Console.WriteLine("Connection accepted. Opening a child socket!");

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
                foreach (var line in requestLines)
                {
                    Console.WriteLine(line);
                }

                string response = BuildHttpResponse(200, "OK", "<html><body><h1>Welcome to my server!</h1></body></html>");
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                client.Send(responseBytes);

                client.Close();
            });

            childSocketThread.Start();
        }
    }

    private string BuildHttpResponse(int statusCode, string statusMessage, string body)
    {
        string statusLine = $"HTTP/1.1 {statusCode} {statusMessage}\r\n";
        string headers = $"Content-Type: text/html\r\nContent-Length: {Encoding.UTF8.GetByteCount(body)}\r\n\r\n";
        return statusLine + headers + body;
    }
}
