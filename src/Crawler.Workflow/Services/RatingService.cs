using System;
using System.Collections.Generic;
using System.IO;
using Crawler.Workflow.Models;
using HtmlAgilityPack;
using NLog;
using System.Linq;

namespace Crawler.Workflow.Services
{
    public interface IRatingService
    {
        IEnumerable<Rating> GetRatings(KeyValuePair<Page, string> page);
    }

    public class RatingService: IRatingService
    {
        private readonly IStorageService storageService;
        private readonly ILogger logger;

        public RatingService(IStorageService storageService, ILogger logger)
        {
            this.storageService = storageService;
            this.logger = logger;
        }

        public IEnumerable<Rating> GetRatings(KeyValuePair<Page, string> page)
        {
            IList<Rating> ratings = new List<Rating>();

            try
            {
                HtmlDocument htmlDocument = new HtmlDocument();

                htmlDocument.LoadHtml(page.Value);

                var nodes = htmlDocument.DocumentNode.SelectNodes("//p");

                if (nodes != null)
                {
                    var subwords = GetPersons();

                    var text = string.Empty;

                    foreach (Subword subword in subwords)
                    {
                            foreach (HtmlNode node in nodes)
                            {
                                text = node.InnerText;
                                if (text.Contains(subword.Name))
                                {
                                    AddRating(ratings, page.Key, subword);
                                }
                            }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
            }

            return ratings;
        }

        private IEnumerable<Subword> GetPersons()
        {
            return storageService.GetSubwords();
        }

        private void AddRating(IList<Rating> ratings, Page page, Subword subword)
        {
            var rating = ratings.SingleOrDefault(x => x.KeywordId == subword.KeywordId);

            if (rating == null)
            {
                rating = new Rating() { PageId = page.PageId, KeywordId = subword.KeywordId, JoinDate = DateTime.Today };
                ratings.Add(rating);
            }
        }

    }
}