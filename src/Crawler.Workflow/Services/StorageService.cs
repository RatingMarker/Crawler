using System;
using System.Collections.Generic;
using Crawler.Workflow.ExternMicroservices;
using Crawler.Workflow.Models;

namespace Crawler.Workflow.Services
{
    public interface IStorageService
    {
        IEnumerable<Site> GetSites();
        IEnumerable<Page> GetPagesByState(int id, int state);
        void AddPage(Page page);
        void UpdatePage(Page page);
    }

    public class StorageService: IStorageService
    {
        private readonly IPageMicroservice pageMicroservice;

        public StorageService(IPageMicroservice pageMicroservice)
        {
            if (pageMicroservice == null)
            {
                throw new ArgumentNullException(nameof(pageMicroservice));
            }
            this.pageMicroservice = pageMicroservice;
        }

        public IEnumerable<Site> GetSites() => pageMicroservice.GetSites();

        public IEnumerable<Page> GetPagesByState(int id, int state) => pageMicroservice.GetPagesbyState(id, state);

        public void AddPage(Page page) => pageMicroservice.PostPage(page);

        public void UpdatePage(Page page) => pageMicroservice.PutPage(page);
    }
}