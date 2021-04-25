using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CommandManager.ThreadPool;
using SocketWebServer;

namespace DdosWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var threadPool = MyThreadPool.GetInstance();
            Enumerable.Range(0, 100).ToList().ForEach(_ => threadPool.Add(TestConnection));
        }

        private static void TestConnection()
        {
            using var socket = GetSocket();
            socket.Send(Encoding.UTF8.GetBytes("GET / HTTP/1.1"));
            socket.Close();
        }

        private static Socket GetSocket()
        {
            Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(IPAddress.Loopback, Config.FileServerPort);
            return socket;
        }
    }
}