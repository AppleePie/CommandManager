using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CommandManager.ThreadPool;

namespace SocketWebClient
{
    internal static class Program
    {
        private const string Host = "127.0.0.1";
        private const int HostPort = 8081;
        private static readonly WebClient WebClient = new();
        
        private static void Main()
        {
            var threadPool = new MyThreadPool();
            
            var newSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            newSocket.Bind(new IPEndPoint(IPAddress.Loopback, 8080));
            newSocket.Listen(10);
            
            while (true)
            {
                var connectedSocket = newSocket.Accept();
                threadPool.Add(() =>
                {
                    var data = WebClient.GetData(IPAddress.Parse(Host), HostPort, "/index.html");
                    connectedSocket.Send(Encoding.UTF8.GetBytes(data), data.Length, 0);
                    connectedSocket.Close();
                });
            }
        }
    }
}