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
        private readonly IRatingService ratingService;
        private readonly IStorageService storageService;
        private readonly IUrlService urlService;

        public CrawlerProcessing(
            IStorageService storageService,
            IDownloadService downloadService,
            IUrlService urlService,
            IRatingService ratingService,
            ILogger logger)
        {
            this.storageService = storageService;
            this.downloadService = downloadService;
            this.urlService = urlService;
            this.ratingService = ratingService;
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

            logger.Info("Detecting keywords");

            var ratings = DetectingKeywords(downloadPage);

            logger.Info("Save new rank persons in Database");

            string message = SavePersonRageRankStorage(ratings);

            logger.Info("Saved rank persons: " + message);

            logger.Info("Update current page");

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

        private string SavePersonRageRankStorage(IEnumerable<Rating> ratings)
        {
            int count = ratings.Count();
            int countSaved = storageService.AddRatings(ratings);

            return $"{countSaved} / {count}";
        }

        private IEnumerable<Rating> DetectingKeywords(KeyValuePair<Page, string> downloadPage)
        {
            return ratingService.GetRatings(downloadPage);
        }
    }
}