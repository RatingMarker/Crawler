using System;
using System.Collections.Generic;
using System.IO;
using Crawler.Workflow.Models;
using Crawler.Workflow.Parsers;
using System.Linq;

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
                var ext = Path.GetExtension(page.Key.Url);

                switch (ext)
                {
                    case ".txt":
                        urls = ParsingUrls(new RobotsTxtParser(), page.Value);
                        break;
                    case ".xml":
                        urls = ParsingUrls(new SitemapParser(), page.Value);
                        urls = RemoveExtGzipUrls(urls);
                        break;
                    default:
                        urls = ParsingUrls(new HtmlParser(), page.Value);
                        urls = FilteringUrls(urls, page.Key);
                        urls = RemoveDuplicateUrls(urls);
                        break;
                }
            }

            return urls;
        }

        private IEnumerable<string> ParsingUrls(IParser parser, string content)
        {
            IEnumerable<string> urls = new List<string>();

            if (parser != null)
            {
                urls = parser.GetUrls(content);
            }

            return urls;
        }

        private IEnumerable<string> RemoveExtGzipUrls(IEnumerable<string> urls)
        {
            var removeExt = ".gz".ToCharArray();

            return urls.Select(x => x.TrimEnd(removeExt));
        }

        private IEnumerable<string> FilteringUrls(IEnumerable<string> urls, Page page)
        {
            var pageUrl = page.Url;

            var newUrls = new List<string>();

            try
            {
                var filteringUrls = FilteringDomain(urls, pageUrl);

                newUrls.AddRange(filteringUrls);

                filteringUrls = FilteringScheme(urls, pageUrl);

                newUrls.AddRange(filteringUrls);

                filteringUrls = BuildingAbsoluteUrls(urls, pageUrl);

                newUrls.AddRange(filteringUrls);

                //                newUrls = newUrls.Where(IsWithoutFile).ToList();
            }
            catch (UriFormatException exception)
            {
                //ignored
            }

            return newUrls;
        }

        private IEnumerable<string> RemoveDuplicateUrls(IEnumerable<string> urls)
        {
            return urls.Distinct().ToList();
        }

        private IEnumerable<string> FilteringDomain(IEnumerable<string> urls, string page)
        {
            var uri = new Uri(page);

            var baseUrl = uri.Scheme + Uri.SchemeDelimiter + uri.Host + "/";

            return urls.Where(url => url.StartsWith(baseUrl)).ToList();
        }

        private IEnumerable<string> FilteringScheme(IEnumerable<string> urls, string page)
        {
            var uri = new Uri(page);

            var baseUrl = uri.Scheme + Uri.SchemeDelimiter + "www." + uri.Host + "/";

            return urls.Where(url => url.StartsWith(baseUrl)).ToList();
        }

        private IEnumerable<string> BuildingAbsoluteUrls(IEnumerable<string> urls, string page)
        {
            var uri = new Uri(page);

            var absoluteUrls = new List<string>();

            foreach (string url in urls)
            {
                if (IsRelativeUrl(url))
                {
                    try
                    {
                        var absoluteUri = new Uri(uri, url).AbsoluteUri;
                        absoluteUrls.Add(absoluteUri);
                    }
                    catch (UriFormatException exception)
                    {
                        // ignored
                    }
                }
            }

            return absoluteUrls;
        }

        protected virtual bool IsRelativeUrl(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Relative);
        }

        protected virtual bool IsWithoutFile(string url)
        {
            bool isFile = true;

            if (Path.HasExtension(url))
            {
                isFile = url.EndsWith("shtml");
            }

            return isFile;
        }
    }
}