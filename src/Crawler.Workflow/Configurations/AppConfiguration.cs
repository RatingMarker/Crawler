using Microsoft.Extensions.Configuration;

namespace Crawler.Workflow.Configurations
{
    public interface IAppConfiguration
    {
        string Get(string property);
    }

    public class AppConfiguration: IAppConfiguration
    {
        private readonly IConfiguration configuration;

        public AppConfiguration()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("config.json");

            configuration = config.Build();
        }

        public string Get(string property)
        {
            return configuration[property];
        }
    }
}