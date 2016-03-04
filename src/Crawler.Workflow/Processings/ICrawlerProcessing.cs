using System.Collections.Generic;
using Crawler.Workflow.Models;

namespace Crawler.Workflow.Processings
{
    public interface ICrawlerProcessing
    {
        void InitializeProcession(IEnumerable<Page> pages);
    }
}