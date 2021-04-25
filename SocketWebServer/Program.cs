using System.Net;
using System.Net.Sockets;
using CommandManager.ThreadPool;

namespace SocketWebServer
{
    internal static class Program
    {
        private static readonly MyThreadPool ThreadPool = MyThreadPool.GetInstance();
        
        private static void Main()
        {
            var serverHostSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var ip = new IPEndPoint(IPAddress.Loopback, Config.FileServerPort);
            serverHostSocket.Bind(ip);
            serverHostSocket.Listen(10);
            
            var cliSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var cliIp = new IPEndPoint(IPAddress.Loopback, Config.CliRemotePort);
            cliSocket.Bind(cliIp);
            cliSocket.Listen();
            
            ThreadPool.Add(() => Server.ListenCliRemotePort(cliSocket));
            ThreadPool.Add(() => Server.ListenFileWebServerPort(serverHostSocket));
        }
    }
}