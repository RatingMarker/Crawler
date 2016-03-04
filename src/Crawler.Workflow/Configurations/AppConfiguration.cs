using Microsoft.Extensions.Configuration;

namespace Crawler.Workflow.Configurations
{
    public interface IConfigurationApp
    {
        string Get(string property);
    }

    public class ConfigurationApp: IConfigurationApp
    {
        private readonly IConfiguration configuration;

        public ConfigurationApp()
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