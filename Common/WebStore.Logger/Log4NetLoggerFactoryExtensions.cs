using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    public static class Log4NetLoggerFactoryExtensions
    {
        private static string NormalizeConfigFilePath(string configFile)
        {
            if (string.IsNullOrEmpty(configFile))
                throw new ArgumentNullException(nameof(configFile));

            if (!Path.IsPathRooted(configFile))
            {
                var assembly = Assembly.GetEntryAssembly();
                var programDir = Path.GetDirectoryName(assembly.Location);
                configFile = Path.Combine(programDir, configFile);
            }

            if (!File.Exists(configFile))
                throw new FileNotFoundException($"Не найден файл конфигурации '{configFile}'", configFile);

            return configFile;
        }

        public static ILoggerFactory AddLog4Net(this ILoggerFactory factory, string configFile = "log4net.config")
        {
            var configFilePath = NormalizeConfigFilePath(configFile);
            factory.AddProvider(new Log4NetLoggerProvider(configFilePath));

            return factory;
        }

        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, string configFile = "log4net.config")
        {
            var configFilePath = NormalizeConfigFilePath(configFile);
            builder.AddProvider(new Log4NetLoggerProvider(configFilePath));

            return builder;
        }
    }
}
