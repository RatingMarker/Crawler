using System;

namespace Crawler.Workflow.ViewModels
{
    public class PageViewModel
    {
        public int PageId { get; set; }
        public string Url { get; set; }
        public DateTime? FoundDate { get; set; }
        public DateTime? LastScanDate { get; set; }
        public int SiteId { get; set; }
    }
}