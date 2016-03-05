using System;
using System.Collections.Generic;

namespace Crawler.Workflow.Models
{
    public class Page
    {
        public int PageId { get; set; }
        public string Url { get; set; }
        public DateTime? FoundDate { get; set; }
        public DateTime? LastScanDate { get; set; }
        public int SiteId { get; set; }
        public virtual Site Site { get; set; }
    }
}