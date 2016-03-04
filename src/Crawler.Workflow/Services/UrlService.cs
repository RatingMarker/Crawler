using System.Collections.Generic;
using System.Linq;
using Crawler.Workflow.Models;
using Crawler.Workflow.Parsers;

namespace Crawler.Workflow.Services
{
    public interface IUrlService
    {
        IEnumerable<string> GetUrls(KeyValuePair<Page, string> page);
    }

    public class UrlService: IUrlService
    {
        public IEnumerable<string> GetUrls(KeyValuePair<Page, string> page)
        {
            IEnumerable<string> urls = new List<string>();

            var content = page.Value;

            if (page.Key != null
                && !string.IsNullOrEmpty(content))
            {
                IParser parser = new SitemapParser();
                urls = parser.GetUrls(page.Value);
                urls = RemoveExtGzipUrls(urls);
            }

            return urls;
        }

        private IEnumerable<string> RemoveExtGzipUrls(IEnumerable<string> urls)
        {
            var removeExt = ".gz".ToCharArray();

            return urls.Select(x => x.TrimEnd(removeExt));
        }
    }
}