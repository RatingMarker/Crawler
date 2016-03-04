using System.Collections.Generic;

namespace Crawler.Workflow.Models
{
    public class Keyword
    {
        public int KeywordId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Subword> Subwords { get; set; }
    }
}