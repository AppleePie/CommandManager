using System.Net;
using System.Net.Sockets;
using CommandManager.ThreadPool;

namespace SocketWebServer
{
    internal static class Program
    {
        private static readonly WebServer WebServer = new();
        
        private static void Main()
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ip = new IPEndPoint(IPAddress.Loopback, 8081);
            socket.Bind(ip);
            socket.Listen(10);
            var threadPool = new MyThreadPool();

            while (true)
            {
                var connectedSocket = socket.Accept();
                threadPool.Add(() => WebServer.SendResponseForRequest(connectedSocket));
            }
        }
    }
}