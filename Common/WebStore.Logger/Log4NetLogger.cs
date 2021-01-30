using System;
using log4net;
using Microsoft.Extensions.Logging;

namespace WebStore.Logger
{
    internal class Log4NetLogger : ILogger
    {
        private readonly ILog _log;

        public Log4NetLogger(ILog log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            //Не поддерживаем работу с областями 
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return _log.IsFatalEnabled;
                case LogLevel.Error:
                    return _log.IsErrorEnabled;
                case LogLevel.Warning:
                    return _log.IsWarnEnabled;
                case LogLevel.Information:
                    return _log.IsInfoEnabled;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    return _log.IsDebugEnabled;
                case LogLevel.None:
                    return false;
                default:
                    throw new NotSupportedException($"{nameof(logLevel)} = {logLevel}");
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var message = formatter(state, exception);
            if (string.IsNullOrEmpty(message) && exception == null)
                return;

            switch (logLevel)
            {
                case LogLevel.Critical:
                    _log.Fatal(message, exception);
                    break;
                case LogLevel.Error:
                    _log.Error(message, exception);
                    break;
                case LogLevel.Warning:
                    _log.Warn(message, exception);
                    break;
                case LogLevel.Information:
                    _log.Info(message, exception);
                    break;
                case LogLevel.Debug:
                case LogLevel.Trace:
                    _log.Debug(message, exception);
                    break;
                default:
                    throw new NotSupportedException($"{nameof(logLevel)} = {logLevel}");
            }

        }
    }
}
