using Autofac;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Crawler.Workflow.Configurations
{
    public class NLogModule: Module
    {
        public NLogModule()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = @"${time}|${level}|${message}"
            };
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget()
            {
                Layout = @"${time}|${level}|${message}",
                FileName = "Log.txt"
            };
            config.AddTarget("file", fileTarget);

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            LogManager.Configuration = config;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => LogManager.GetLogger(GetType().Namespace)).As<ILogger>();
        }
    }
}