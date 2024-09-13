using System.Runtime.CompilerServices;

namespace HttpServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ListeningSocket ListeningServer = new ListeningSocket();
            ListeningServer.StartListeningSocket();
        }
    }
}