using System.Collections.Generic;
using HtmlAgilityPack;

namespace Crawler.Workflow.Parsers
{
    public class HtmlParser:IParser
    {
        public IEnumerable<string> GetUrls(string content)
        {
            ICollection<string> urls = new List<string>();

            if (!string.IsNullOrWhiteSpace(content))
            {
                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(content);

                var nodes = doc.DocumentNode.SelectNodes("//a[@href]");

                if (nodes != null)
                {
                    foreach (HtmlNode node in nodes)
                    {
                        urls.Add(node.Attributes["href"].Value);
                    }
                }
            }

            return urls;
        }
    }
}