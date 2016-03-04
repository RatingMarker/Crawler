using System;
using System.Collections.Generic;
using System.Linq;
using Crawler.Workflow.Models;
using Crawler.Workflow.Services;
using NLog;

namespace Crawler.Workflow.Processings
{
    internal class CrawlerProcessing: ICrawlerProcessing
    {
        private readonly IDownloadService downloadService;
        private readonly ILogger logger;
        private readonly IStorageService storageService;
        private readonly IUrlService urlService;

        public CrawlerProcessing(
            IStorageService storageService,
            IDownloadService downloadService,
            IUrlService urlService,
            ILogger logger)
        {
            this.storageService = storageService;
            this.downloadService = downloadService;
            this.urlService = urlService;
            this.logger = logger;
        }

        public void InitializeProcession(IEnumerable<Page> pages)
        {
            foreach (Page page in pages)
            {
                ProcessingPage(page);
            }
        }

        private void ProcessingPage(Page page)
        {
            var currentPage = page;

            logger.Debug("Download page");

            var downloadPage = Download(page);

            logger.Debug("Download completed");

            logger.Debug("Parsing urls");

            var urls = FetchingUrlsByPage(downloadPage);

            logger.Debug("Founded urls: " + urls.Count());

            logger.Debug("Creating pages");

            var newPages = CreatePages(currentPage.SiteId, urls);

            logger.Debug("Save new pages in Database");

            string countSaved = SavePagesStorage(newPages);

            logger.Debug("Saved pages: " + countSaved);

            UpdateLastScanDatePage(currentPage);
        }

        private KeyValuePair<Page, string> Download(Page page)
        {
            var content = downloadService.Download(page.Url);

            page.LastScanDate = DateTime.Now;

            var downloadPage = new KeyValuePair<Page, string>(page, content);

            return downloadPage;
        }

        private void UpdateLastScanDatePage(Page page)
        {
            storageService.UpdatePage(page);
        }

        private IEnumerable<string> FetchingUrlsByPage(KeyValuePair<Page, string> page)
        {
            return urlService.GetUrls(page);
        }

        private IEnumerable<Page> CreatePages(int siteId, IEnumerable<string> urls)
        {
            ICollection<Page> pages = new List<Page>();

            foreach (string url in urls)
            {
                var page = new Page()
                {
                    Url = url,
                    SiteId = siteId,
                    FoundDate = DateTime.Now
                };

                pages.Add(page);
            }

            return pages;
        }

        private string SavePagesStorage(IEnumerable<Page> pages)
        {
            return storageService.AddPages(pages);
        }
    }
}