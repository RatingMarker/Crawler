using System;
using Autofac;
using Crawler.Workflow;
using Crawler.Workflow.Containers;
using Crawler.Workflow.ExternMicroservices;
using Crawler.Workflow.Processings;
using Crawler.Workflow.Services;
using NLog;

namespace Crawler.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var scope = AutofacContainer.Get().BeginLifetimeScope())
            {
                var crawlerManage = scope.Resolve<CrawlerManage>();

                crawlerManage.InitializeCrawler();
            }
        }
    }
}