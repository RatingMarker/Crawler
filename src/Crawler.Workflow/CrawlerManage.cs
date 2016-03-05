using System;
using System.Collections.Generic;
using System.Linq;
using Crawler.Workflow.Models;
using Crawler.Workflow.Processings;
using Crawler.Workflow.Services;
using NLog;

namespace Crawler.Workflow
{
    public class CrawlerManage
    {
        private readonly ICrawlerProcessing crawlerProcessing;
        private readonly ILogger logger;
        private readonly IStorageService storageService;
        private bool isProcessing;
        private IEnumerable<Site> sites;

        public CrawlerManage(
            ICrawlerProcessing crawlerProcessing,
            IStorageService storageService,
            ILogger logger)
        {
            if (crawlerProcessing == null)
            {
                throw new ArgumentNullException(nameof(crawlerProcessing));
            }
            if (storageService == null)
            {
                throw new ArgumentNullException(nameof(storageService));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            this.crawlerProcessing = crawlerProcessing;
            this.storageService = storageService;
            this.logger = logger;
        }

        public void InitializeCrawler()
        {
            ProcessingCrawler();
        }

        public void ProcessingCrawler()
        {
            isProcessing = true;

            logger.Info("Crawler starting");

            sites = storageService.GetSites();

            ProcessingSites();

            isProcessing = false;

            logger.Info("Crawler completed");
        }

        private void ProcessingSites()
        {
            foreach (var site in sites)
            {
                //Get pages with x => x.SiteId == id && x.LastScanDate == null;
                var pages = storageService.GetPagesByState(site.SiteId, 2);

                logger.Debug("Get pages: {0}",pages.Count());

                crawlerProcessing.InitializeProcession(pages);
            }
        }
    }
}