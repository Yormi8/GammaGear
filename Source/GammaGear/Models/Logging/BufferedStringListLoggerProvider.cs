using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace GammaGear.Logging
{
    public sealed class BufferedStringListLoggerProvider : ILoggerProvider
    {
        public readonly ConcurrentQueue<Log> LogQueue = new();
        private readonly ConcurrentDictionary<string, BufferedStringListLogger> _loggers =
        new(StringComparer.OrdinalIgnoreCase);

        public BufferedStringListLoggerProvider() { }

        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new BufferedStringListLogger(name, LogQueue));

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
