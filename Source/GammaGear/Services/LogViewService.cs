using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using GammaGear.Configuration;
using GammaGear.Logging;
using GammaGear.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GammaGear.Services
{
    public class LogViewService : ILogViewService
    {
        public ObservableCollection<Log> LogMessages { get; } = new ObservableCollection<Log>();
        private readonly object _lock = new object();
        private readonly BufferedStringListLoggerProvider _loggerProvider;
        private DispatcherTimer _timer;
        private readonly int _trimThreshold;

        public LogViewService(IOptions<LogSettings> logSettings, [FromKeyedServices("BufferedStringListLoggerProvider")] ILoggerProvider loggerProvider)
        {
            if (loggerProvider is not BufferedStringListLoggerProvider)
                throw new ArgumentException($"Got {nameof(loggerProvider)} when BufferedStringListLoggerProvider was expected", nameof(loggerProvider));

            _loggerProvider = loggerProvider as BufferedStringListLoggerProvider;

            var settings = logSettings.Value;
            _trimThreshold = settings.TrimThreshold;

            // Initialize DispatcherTimer on the main UI thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(settings.RefreshIntervalMs) };
                _timer.Tick += (s, e) => FlushBufferToUI();
                _timer.Start();
            });
        }

        private void FlushBufferToUI()
        {
            lock (_lock)
            {
                foreach (var log in _loggerProvider.LogQueue)
                {
                    LogMessages.Add(log);
                }
                _loggerProvider.LogQueue.Clear();

                // Optional: Trim collection to prevent memory overflow
                if (LogMessages.Count > _trimThreshold)
                {
                    int overflow = LogMessages.Count - _trimThreshold;
                    for (int i = 0; i < overflow; i++)
                    {
                        LogMessages.RemoveAt(0);
                    }
                }
            }
        }
    }
}
