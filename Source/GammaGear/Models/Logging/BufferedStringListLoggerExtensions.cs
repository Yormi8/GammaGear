using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging;

namespace GammaGear.Logging
{
    public static class BufferedStringListLoggerExtensions
    {
        public static ILoggingBuilder AddBufferedStringListLogger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, BufferedStringListLoggerProvider>());

            //LoggerProviderOptions.RegisterProviderOptions
            //    <ColorConsoleLoggerConfiguration, BufferedStringListLoggerProvider>(builder.Services);

            return builder;
        }

        //public static ILoggingBuilder AddBufferedStringListLogger(this ILoggingBuilder builder)
        //{
        //    builder.AddBufferedStringListLogger();
        //    builder.Services.Configure(configure);
        //
        //    return builder;
        //}
    }
}
