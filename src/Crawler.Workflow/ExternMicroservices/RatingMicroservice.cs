using System.Collections.Generic;
using Crawler.Workflow.Configurations;
using Crawler.Workflow.Models;
using Crawler.Workflow.ViewModels;
using Mapster;
using RestSharp;

namespace Crawler.Workflow.ExternMicroservices
{
    public interface IRatingMicroservice
    {
        void Add(Rating rating);
        void PostRatings(IEnumerable<Rating> ratings);
    }

    public class RatingMicroservice: IRatingMicroservice
    {
        private readonly IAdapter adapter;
        private readonly IRestClient client;

        public RatingMicroservice(IAppConfiguration config, IAdapter adapter)
        {
            this.adapter = adapter;

            string baseUrl = config.Get("Url:RatingMicroservice");
            client = new RestClient(baseUrl);
        }

        public void Add(Rating rating)
        {
            var data = adapter.Adapt<RatingViewModel>(rating);
            var request = new RestRequest("/api/ratings", Method.POST);
            request.AddJsonBody(data);
            client.Execute(request);
        }

        public void PostRatings(IEnumerable<Rating> ratings)
        {
            var data = adapter.Adapt<IEnumerable<RatingViewModel>>(ratings);
            var request = new RestRequest("/api/ratings/insert", Method.POST);
            request.AddJsonBody(data);
            client.Execute(request);
        }
    }
}