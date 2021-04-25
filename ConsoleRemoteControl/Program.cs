using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ConsoleRemoteControl
{
    internal static class Program
    {
        private const int BufferSize = 1024;

        private static void Main()
        {
            while (true)
            {
                using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    var command = Console.ReadLine();
                    socket.Connect(new IPEndPoint(IPAddress.Loopback, 8081));
                
                    socket.Send(Encoding.UTF8.GetBytes(command));
                    var buffer = new byte[BufferSize];
                    var length = socket.Receive(buffer);
                    Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, length));
                }
                finally { socket.Close(); }
            }
        }
    }
}