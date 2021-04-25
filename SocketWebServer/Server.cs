using System.Net.Sockets;
using System.Text;
using CommandManager.ThreadPool;

namespace SocketWebServer
{
    public static class Server
    {
        public static bool IsActive { get; set; } = true;
        private static readonly FileWebServer FileWebServer = new(Config.RootPath);
        private static readonly CliHandler CliHandler = new(Config.RootPath);
        
        public static void ListenFileWebServerPort(Socket socket)
        {
            var threadPool = MyThreadPool.GetInstance();
            try
            {
                while (IsActive)
                {
                    var connectedSocket = socket.Accept();
                    threadPool.Add(() => FileWebServer.SendResponseForRequest(connectedSocket));
                }
            }
            finally { socket.Close(); }
        }

        public static void ListenCliRemotePort(Socket cliSocket)
        {
            while (true)
            {
                Socket cliConnectedSocket = default;
                try
                {
                    cliConnectedSocket = cliSocket.Accept();
                    var response = CliHandler.ExecuteCommand(cliConnectedSocket);
                    cliConnectedSocket.Send(Encoding.UTF8.GetBytes(response));
                }
                catch { /* */ }
                finally { cliConnectedSocket?.Close(); }
            }
        }
    }
}