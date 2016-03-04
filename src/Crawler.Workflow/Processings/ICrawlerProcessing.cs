using System;
using System.Collections.Generic;
using Crawler.Workflow.Models;
using Crawler.Workflow.Services;
using NLog;
using System.Linq;

namespace Crawler.Workflow.Processings
{
    public interface ICrawlerProcessing
    {
        void InitializeProcession(IEnumerable<Page> pages);
    }
}