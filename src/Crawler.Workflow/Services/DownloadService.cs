using System;
using Crawler.Workflow.Dowmloaders;

namespace Crawler.Workflow.Services
{
    public interface IDownloadService
    {
        string Download(string url);
    }

    public class DownloadService: IDownloadService
    {
        private readonly IDownloader downloader;

        public DownloadService(IDownloader downloader)
        {
            this.downloader = downloader;
        }

        public string Download(string url)
        {
            var content = string.Empty;

            if (IsUrl(url))
            {
                content = DownloadContent(url);
            }

            return content;
        }

        private string DownloadContent(string url)
        {
            return downloader.Download(url);
        }

        private bool IsUrl(string url)
        {
            bool isUrl = false;

            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    var uri = new Uri(url);
                    isUrl = uri.IsAbsoluteUri;
                }
                catch (UriFormatException)
                {
                    // ignore
                }
            }

            return isUrl;
        }
    }
}