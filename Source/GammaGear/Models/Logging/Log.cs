using Microsoft.Extensions.Logging;

namespace GammaGear.Logging
{
    public readonly struct Log
    {
        public LogLevel LogLevel { get; init; }
        public string Message { get; init; }
    }
}

