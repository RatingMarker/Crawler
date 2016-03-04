using Autofac;
using Crawler.Workflow.Dowmloaders;
using Crawler.Workflow.ExternMicroservices;
using Crawler.Workflow.Processings;
using Crawler.Workflow.Services;
using Mapster;
using Microsoft.Extensions.Configuration;

namespace Crawler.Workflow.Configurations
{
    public class AutofacModule: Module
    {
        private readonly IConfiguration configuration;

        public AutofacModule()
        {
            var config = new ConfigurationBuilder();
            config.AddJsonFile("config.json");

            configuration = config.Build();
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegistryLogging(builder);
            RegistryConfiguration(builder);
            RegistryServices(builder);
            RegistryMicroservices(builder);
            RegistryProcessing(builder);
            RegistryOther(builder);
        }

        private void RegistryLogging(ContainerBuilder builder)
        {
            builder.RegisterModule<NLogModule>();
        }

        private void RegistryConfiguration(ContainerBuilder builder)
        {
            builder.RegisterType<ConfigurationApp>().As<IConfigurationApp>();
        }

        private void RegistryServices(ContainerBuilder builder)
        {
            builder.RegisterType<DownloadService>().As<IDownloadService>();
            builder.RegisterType<StorageService>().As<IStorageService>();
            builder.RegisterType<UrlService>().As<IUrlService>();
        }

        private void RegistryMicroservices(ContainerBuilder builder)
        {
            builder.RegisterType<PageMicroservice>().As<IPageMicroservice>();
        }

        private void RegistryOther(ContainerBuilder builder)
        {

            builder.RegisterType<Adapter>().As<IAdapter>();
            builder.RegisterType<RestDownloader>().As<IDownloader>();
        }

        private void RegistryProcessing(ContainerBuilder builder)
        {
            builder.RegisterType<CrawlerManage>();
            builder.RegisterType<CrawlerProcessing>().As<ICrawlerProcessing>();
        }
    }
}