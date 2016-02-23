using System;

namespace Crawler.Workflow.Models
{
    public class PersonPageRank
    {
        public int PersonPageRankId { get; set; }
        public int PersonId { get; set; }
        public virtual Keyword Keyword { get; set; }
        public int PageId { get; set; }
        public virtual Page Page { get; set; }
        public int Rank { get; set; }
        public DateTime JoinDate { get; set; }
    }
}