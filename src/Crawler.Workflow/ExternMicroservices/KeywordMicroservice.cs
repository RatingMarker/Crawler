using System.Collections.Generic;
using Crawler.Workflow.Configurations;
using Crawler.Workflow.Models;
using Crawler.Workflow.ViewModels;
using Mapster;
using RestSharp;

namespace Crawler.Workflow.ExternMicroservices
{
    public interface IKeywordMicroservice
    {
        IEnumerable<Subword> GetAllSubwords();
    }

    public class KeywordMicroservice: IKeywordMicroservice
    {
        private readonly IAppConfiguration config;
        private readonly IAdapter adapter;
        private readonly IRestClient client;

        public KeywordMicroservice(IAppConfiguration config, IAdapter adapter)
        {
            this.adapter = adapter;

            string baseUrl = config.Get("Url:KeywordMicroservice");
            client = new RestClient(baseUrl);
        }

        public IEnumerable<Subword> GetAllSubwords()
        {
            var request = new RestRequest("/api/subwords", Method.GET);
            var response = client.Execute<List<SubwordViewModel>>(request);
            var data = response.Data;
            return adapter.Adapt<IEnumerable<Subword>>(data);
        } 
    }
}