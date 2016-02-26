using System;
using System.Collections.Generic;
using Crawler.Workflow.Configurations;
using Crawler.Workflow.Models;
using Crawler.Workflow.ViewModels;
using Mapster;
using RestSharp;

namespace Crawler.Workflow.ExternMicroservices
{
    public interface IPageMicroservice
    {
        IEnumerable<Site> GetSites();
        IEnumerable<Page> GetPagesbySite(int siteId);
        IEnumerable<Page> GetPagesbyState(int siteId, int state);
        int PostPages(IEnumerable<Page> pages);
        void PutPage(Page page);
        void PostPage(Page page);
    }

    public class PageMicroservice: IPageMicroservice
    {
        private readonly IAdapter adapter;
        private readonly IRestClient client;

        public PageMicroservice(IAppConfiguration config, IAdapter adapter)
        {
            this.adapter = adapter;

            string baseUrl = config.Get("Url:PageMicroservice");
            client = new RestClient(baseUrl);
        }

        public IEnumerable<Site> GetSites()
        {
            var request = new RestRequest("/api/sites", Method.GET);
            var response = client.Execute<List<SiteViewModel>>(request);
            var data = response.Data;
            return adapter.Adapt<IEnumerable<Site>>(data);
        }

        public IEnumerable<Page> GetPagesbySite(int siteId)
        {
            var request = new RestRequest("api/sites/{id}/pages", Method.GET);
            request.AddParameter("id", siteId, ParameterType.UrlSegment);
            var response = client.Execute<List<PageViewModel>>(request);
            var data = response.Data;
            return adapter.Adapt<IEnumerable<Page>>(data);
        }

        public IEnumerable<Page> GetPagesbyState(int siteId, int state)
        {
            var request = new RestRequest("api/sites/{id}/pages/{state}", Method.GET);
            request.AddParameter("id", siteId, ParameterType.UrlSegment);
            request.AddParameter("state", state, ParameterType.UrlSegment);
            var response = client.Execute<List<PageViewModel>>(request);
            var data = response.Data;
            return adapter.Adapt<IEnumerable<Page>>(data);
        }

        public int PostPages(IEnumerable<Page> pages)
        {
            throw new System.NotImplementedException();
        }

        public void PutPage(Page page)
        {
            int id = page.PageId;
            var data = adapter.Adapt<PageViewModel>(page);
            var request = new RestRequest("api/pages/{id}", Method.PUT);
            request.AddParameter("id", id, ParameterType.UrlSegment);
            request.AddJsonBody(data);
            client.Execute<List<PageViewModel>>(request);
        }

        public void PostPage(Page page)
        {
            var data = adapter.Adapt<PageViewModel>(page);
            var request = new RestRequest("api/pages", Method.POST);
            request.AddJsonBody(data);
            client.Execute<List<PageViewModel>>(request);
        }
    }
}