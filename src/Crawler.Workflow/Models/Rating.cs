using System;

namespace Crawler.Workflow.Models
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int KeywordId { get; set; }
        public int PageId { get; set; }
        public int SiteId { get; set; }
        public DateTime JoinDate { get; set; }
    }
}