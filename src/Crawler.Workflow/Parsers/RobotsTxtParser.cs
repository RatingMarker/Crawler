using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static System.String;

namespace Crawler.Workflow.Parsers
{
    public interface IRobotsTxtParser
    {
        IEnumerable<string> GetSitemaps(string content);
    }

    public class RobotsTxtParser: IRobotsTxtParser
    {
        public IEnumerable<string> GetSitemaps(string content)
        {
            ICollection<string> sitemaps = new Collection<string>();

            string[] lines = content.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (line.Contains("Sitemap"))
                {
                    var url = GetValue(line);
                    if (url.EndsWith(".xml"))
                    {
                        sitemaps.Add(url);
                    }
                }
            }

            return sitemaps.Distinct();
        }

        private string GetValue(string line)
        {
            string url = Empty;

            var index = line.IndexOf(':');
            if (index == -1)
            {
                return Empty;
            }

            url = line.Substring(index + 1).Trim();

            return url;
        }
    }
}