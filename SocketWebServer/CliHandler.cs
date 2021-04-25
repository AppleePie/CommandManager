using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CommandManager.Executors;
using CommandManager.ThreadPool;

namespace SocketWebServer
{
    public class CliHandler
    {
        private readonly string mappedRoot;
        private readonly Md5Executor md5Executor = new();

        public CliHandler(string mappedRoot) => this.mappedRoot = mappedRoot;

        public string ExecuteCommand(Socket cliConnectedSocket)
        {
            string response;
            try
            {
                var (command, args) = ReadCommandFrom(cliConnectedSocket);
                response = HandleCommand(command, args);
            }
            catch (Exception e)
            {
                response = e.Message + Environment.NewLine + "Sorry, please try again";
            }

            return response;
        }

        private string HandleCommand(RemoteCommand command, string args = default)
        {
            var commandNeedsArgs = command is RemoteCommand.Size or RemoteCommand.Hash;
            if (commandNeedsArgs && args is null)
                throw new ArgumentException("Arguments to hash and list commands shouldn't be null!");

            return command switch
            {
                RemoteCommand.List => string.Join(Environment.NewLine,
                    Directory.EnumerateFileSystemEntries(mappedRoot)),
                RemoteCommand.Hash => md5Executor.Execute(args).GetValueOrThrow(),
                RemoteCommand.Size => $"{File.Open(args!, FileMode.Open).Length / 1024.0:F2} KB",
                RemoteCommand.Status => Server.IsActive.ToString(),
                RemoteCommand.Stop => StopServer(),
                RemoteCommand.Start => StartServer(),
                _ => ""
            };

            static string StopServer()
            {
                Server.IsActive = false;
                var serverHostSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var ip = new IPEndPoint(IPAddress.Loopback, Config.FileServerPort);
                serverHostSocket.Connect(ip);
                serverHostSocket.Close();
                return "Server is stopped";
            }
            
            static string StartServer()
            {
                var serverHostSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var ip = new IPEndPoint(IPAddress.Loopback, Config.FileServerPort);
                serverHostSocket.Bind(ip);
                serverHostSocket.Listen(10);
                
                Server.IsActive = true;
                MyThreadPool.GetInstance().Add(() => Server.ListenFileWebServerPort(serverHostSocket));
                return "Server is started";
            }
        }

        private static (RemoteCommand command, string args) ReadCommandFrom(Socket cliConnectedSocket)
        {
            var buffer = new byte[Config.BufferSize];
            var length = cliConnectedSocket.Receive(buffer);
            var rawCommand = Encoding.UTF8.GetString(buffer, 0, length).Split(' ');
            var command = char.ToUpper(rawCommand[0][0]) + rawCommand[0][1..];
            var args = rawCommand.Length > 1 ? rawCommand[1] : default;

            return Enum.TryParse(typeof(RemoteCommand), command, out var result)
                ? ((RemoteCommand) result!, args)
                : throw new ArgumentException("The command is not recognized!");
        }
    }
}