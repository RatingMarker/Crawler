using System.Collections.Generic;

namespace Crawler.Workflow.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
    }
}