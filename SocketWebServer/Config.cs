using CommandManager.ThreadPool;

namespace SocketWebServer
{
    public static class Config
    {
        public const int BufferSize = 1024;
        public const int FileServerPort = 8080;
        public const int CliRemotePort = 8081;
        public static readonly string RootPath = @"D:\";
    }
}