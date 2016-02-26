using System;
using System.Collections.Generic;
using Crawler.Workflow.Models;
using Crawler.Workflow.Services;

namespace Crawler.Workflow.Processings
{
    public interface ICrawlerProcessing
    {
        void InitializeProcession(IEnumerable<Page> pages);
    }

    class CrawlerProcessing : ICrawlerProcessing
    {
        private readonly IStorageService storageService;
        private readonly IDownloadService downloadService;

        public CrawlerProcessing(IStorageService storageService, IDownloadService downloadService)
        {
            this.storageService = storageService;
            this.downloadService = downloadService;
        }

        public void InitializeProcession(IEnumerable<Page> pages)
        {
            foreach (Page page in pages)
            {
                var baseurl = new Uri(page.Url);
                var url = new Uri(baseurl, "robots.txt").AbsoluteUri;
                var content = downloadService.Download(url);
                if (content.Contains("Sitemap"))
                {
                    var newPage = new Page()
                    {
                        FoundDate = DateTime.Now,
                        Url = url,
                        SiteId = page.SiteId
                    };

                    storageService.AddPage(newPage);

                    page.LastScanDate = DateTime.Now;
                }

                page.FoundDate = DateTime.Now;
                storageService.UpdatePage(page);
            }
        }
    }
}