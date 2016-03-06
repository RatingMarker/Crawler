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
            RegistryServices(builder);
            RegistryMicroservices(builder);
            RegistryComponent(builder);
        }

        private void RegistryLogging(ContainerBuilder builder)
        {
            builder.RegisterModule<NLogModule>();
        }

        private void RegistryServices(ContainerBuilder builder)
        {
            builder.RegisterType<DownloadService>().As<IDownloadService>();
            builder.RegisterType<StorageService>().As<IStorageService>();
            builder.RegisterType<UrlService>().As<IUrlService>();
            builder.RegisterType<RatingService>().As<IRatingService>();
        }

        private void RegistryMicroservices(ContainerBuilder builder)
        {
            builder.RegisterType<PageMicroservice>().As<IPageMicroservice>();
            builder.RegisterType<KeywordMicroservice>().As<IKeywordMicroservice>();
            builder.RegisterType<RatingMicroservice>().As<IRatingMicroservice>();
        }

        private void RegistryComponent(ContainerBuilder builder)
        {
            builder.RegisterType<AppConfiguration>().As<IAppConfiguration>();
            builder.RegisterType<Adapter>().As<IAdapter>();
            builder.RegisterType<RestDownloader>().As<IDownloader>();

            builder.RegisterType<CrawlerManage>();
            builder.RegisterType<CrawlerProcessing>().As<ICrawlerProcessing>();
        }
    }
}