using CommandManager;
using CommandManager.Executors;
using CommandManager.Results;

namespace SocketWebServer
{
    public static class DirectoryMapper
    {
        public static string MapDirectoryToHtml(string path, string root)
        {
            var fileWorker = new FileWorker(path, false);
            var fileHandler = new HtmlFileHandler();
            var htmlResult = new HtmlResult(root, path);
            
            return htmlResult
                .DumpResult(
                    nameof(HtmlFileHandler),
                    fileWorker.Process(fileHandler)
                ).GetValueOrThrow();
        }
    }
}