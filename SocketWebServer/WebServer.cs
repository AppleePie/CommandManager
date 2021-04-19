using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SocketWebServer
{
    public class WebServer
    {
        private int BufferSize { get; }
        private string RootPath { get; }

        public WebServer(int bufferSize = 1024, string path = default)
        {
            BufferSize = bufferSize;
            RootPath = path ?? Environment.CurrentDirectory;
        }

        public void SendResponseForRequest(Socket connectedSocket)
        {
            var response = GetBodyForRequest(connectedSocket);
            connectedSocket.Send(response);
            connectedSocket.Close();
        }
        
        private byte[] GetBodyForRequest(Socket connectedSocket)
        {
            var buffer = new byte[BufferSize];
            var bytes = connectedSocket.Receive(buffer);
            var request = ParseRequest(Encoding.UTF8.GetString(buffer, 0, bytes)) ?? RootPath;
            var fullPath = Path.Combine(RootPath, request).Replace('/', '\\');
            
            if (!File.Exists(fullPath))
            {
                var body = DirectoryMapper.MapDirectoryToHtml(request, RootPath);
                var headers = string.Join(Environment.NewLine, 
            
                    "HTTP/1.1 200 OK",
                    "Content-Type: text/html; charset=UTF-8",
                    $"Content-Length: {body.Length}"
                );
                return Encoding.UTF8.GetBytes(headers + Environment.NewLine + Environment.NewLine + body);
            }
            
            var file = new FileInfo(fullPath);
            var fileBody = new byte[file.Length];
            file.OpenRead().Read(fileBody);
            
            var fileHeaders = string.Join(Environment.NewLine, 
            
                "HTTP/1.1 200 OK",
                $"Content-Length: {fileBody.Length}",
                "Connection: close"
            );

            return Encoding.UTF8.GetBytes(fileHeaders + Environment.NewLine + Environment.NewLine + Encoding.UTF8.GetString(fileBody));
        }
        
        private static string ParseRequest(string requestData)
        {
            var request = requestData.Split(Environment.NewLine)
                .First()
                .Split(' ')
                .Skip(1)
                .FirstOrDefault()?
                .Substring(1);
            if (string.IsNullOrEmpty(request) || request.StartsWith("favicon"))
                return null;
            return request;
        }
    }
}