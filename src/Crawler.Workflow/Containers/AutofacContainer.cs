using Autofac;
using Crawler.Workflow.Configurations;

namespace Crawler.Workflow.Containers
{
    public static class AutofacContainer
    {
        private static IContainer container;

        public static IContainer Get()
        {
            return container ?? (container = Build());
        }

        private static IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<AutofacModule>();

            return builder.Build();
        }
    }
}