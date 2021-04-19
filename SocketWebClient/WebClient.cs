using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketWebClient
{
    public class WebClient
    {
        private int BufferSize { get; }

        public WebClient(int bufferSize = 1024) => BufferSize = bufferSize;

        public string GetData(IPAddress address, int port, string resource = default)
        {
            resource ??= "/";
            var header = Encoding.UTF8.GetBytes($"GET {resource} HTTP/1.1");
            var buffer = new byte[BufferSize];

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(address, port);
            socket.Send(header, header.Length, 0);
            var length = socket.Receive(buffer);
            socket.Close();
            
            return Encoding.UTF8.GetString(buffer, 0, length);
        }
    }
}