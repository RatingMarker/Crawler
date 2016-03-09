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
        IEnumerable<string> GetSitemap(string content);
    }

    public class UrlService: IUrlService
    {
        public IEnumerable<string> GetSitemap(string content)
        {
            IRobotsTxtParser parser = new RobotsTxtParser();
            return parser.GetSitemaps(content);
        }
    }
}