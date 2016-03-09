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

            var url = BuildRobotsUrl(currentPage.Url);

            logger.Debug("Download page");

            var downloadContent = Download(url);

            logger.Debug("Download completed");

            if (!string.IsNullOrEmpty(downloadContent))
            {
                logger.Debug("Parsing sitemap");

                var urls = FetchingSitemapByContent(downloadContent);

                logger.Debug("Founded sitemap: " + urls.Count());

                if (urls.Any())
                {
                    logger.Debug("Creating pages");

                    var newPages = CreatePages(currentPage.SiteId, urls);

                    logger.Debug("Save new pages in Database");

                    string countSaved = SavePagesStorage(newPages);

                    logger.Debug("Saved pages: " + countSaved);
                }
            }

            logger.Info("Update current page");

            currentPage.FoundDate = DateTime.Now;

            UpdateLastScanDatePage(currentPage);
        }

        private string BuildRobotsUrl(string baseurl)
        {
            Uri baseUri = new Uri(baseurl);
            return new Uri(baseUri, "/robots.txt").AbsoluteUri;
        }

        private string Download(string url)
        {
            return downloadService.Download(url);
        }

        private IEnumerable<string> FetchingSitemapByContent(string content)
        {
            return urlService.GetSitemap(content);
        }

        private void UpdateLastScanDatePage(Page page)
        {
            storageService.UpdatePage(page);
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