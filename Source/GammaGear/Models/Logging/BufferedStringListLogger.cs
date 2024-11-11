using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Text;
using GammaGear.Configuration;

namespace GammaGear.Logging
{
    public sealed class BufferedStringListLogger : ILogger
    {
        private readonly ConcurrentQueue<Log> _logQueue;
        private readonly string _categoryName;
        private readonly LogLevel _minLogLevel = LogLevel.Trace;

        public BufferedStringListLogger(string categoryName, ConcurrentQueue<Log> logQueue)
        {
            _categoryName = categoryName;
            _logQueue = logQueue;
        }

        // Shouldn't need scope functionality for this
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLogLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel) || formatter == null) return;

            var sb = new StringBuilder();
            sb.Append($"{DateTime.UtcNow:O} [{logLevel}] {_categoryName} - {formatter(state, exception)}");

            if (exception != null)
                sb.Append($" Exception: {exception}");

            _logQueue.Enqueue(new Log { Message = sb.ToString(), LogLevel = logLevel });
        }
    }
}
