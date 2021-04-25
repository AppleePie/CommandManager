using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SocketWebServer
{
    public class FileWebServer
    {
        private readonly string rootPath;
        private static readonly string HeadersEnd = Environment.NewLine + Environment.NewLine;

        public FileWebServer(string path = default) => rootPath = path ?? Environment.CurrentDirectory;

        public void SendResponseForRequest(Socket connectedSocket)
        {
            var response = GetBodyForRequest(connectedSocket);
            connectedSocket.Send(response);
            connectedSocket.Close();
        }
        
        private byte[] GetBodyForRequest(Socket connectedSocket)
        {
            var buffer = new byte[Config.BufferSize];
            var bytes = connectedSocket.Receive(buffer);
            var request = ParseGetRequest(Encoding.UTF8.GetString(buffer, 0, bytes)) ?? rootPath;
            var fullPath = !string.IsNullOrEmpty(request) ? Path.Combine(rootPath, request) : rootPath;
            return File.Exists(fullPath) ? GetFileBody(fullPath) : GetDirectoryAsHtml(fullPath);
        }

        private byte[] GetDirectoryAsHtml(string fullPath)
        {
            var body = DirectoryMapper.MapDirectoryToHtml(fullPath, rootPath);
            var headers = string.Join(Environment.NewLine,
                "HTTP/1.1 200 OK",
                "Content-Type: text/html; charset=UTF-8",
                $"Content-Length: {body.Length}"
            );
            return Encoding.UTF8.GetBytes($"{headers}{HeadersEnd}{body}");
        }

        private static byte[] GetFileBody(string fullPath)
        {
            var file = new FileInfo(fullPath);
            var fileBody = new byte[file.Length];
            
            using var fStream = file.OpenRead();
            fStream.Read(fileBody);
            fStream.Close();

            var fileHeaders = string.Join(Environment.NewLine,
                "HTTP/1.1 200 OK",
                "Content-Type: charset=utf-8",
                $"Content-Length: {fileBody.Length}",
                "Connection: close"
            );

            return Encoding.UTF8.GetBytes($"{fileHeaders}{HeadersEnd}{Encoding.UTF8.GetString(fileBody)}");
        }

        private static string ParseGetRequest(string requestData)
        {
            var request = requestData.Split(Environment.NewLine)
                .First()
                .Split(' ')
                .Skip(1)
                .FirstOrDefault()?
                .Replace("%20", " ")
                .Substring(1);
            if (string.IsNullOrEmpty(request) || request.StartsWith("favicon"))
                return null;
            return request;
        }
    }
}