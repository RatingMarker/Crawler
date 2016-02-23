namespace Crawler.Workflow.Models
{
    public class Subword
    {
        public int SubwordId { get; set; }
        public string Name { get; set; }
        public int PersonId { get; set; }
        public virtual Keyword Person { get; set; }
    }
}