using System.Collections.Concurrent;
using System.IO;
using System.Reflection;
using log4net;
using log4net.Repository;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    internal class Log4NetLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, ILogger> _loggers;
        private readonly ILoggerRepository _loggerRepository;

        public Log4NetLoggerProvider(string configFile)
        {
            _loggers = new ConcurrentDictionary<string, ILogger>();

            _loggerRepository = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(_loggerRepository, new FileInfo(configFile));
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, 
                catName => new Log4NetLogger(LogManager.GetLogger(_loggerRepository.Name, catName)));
        }

        public void Dispose()
        {
            _loggers.Clear();
            _loggerRepository.Shutdown();
        }
    }
}
