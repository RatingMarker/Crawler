using System.Collections.Generic;

namespace Crawler.Workflow.Parsers
{
    public interface IParser
    {
        IEnumerable<string> GetUrls(string content);
    }
}