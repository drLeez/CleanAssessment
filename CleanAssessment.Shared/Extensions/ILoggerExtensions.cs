using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Extensions;
public static class ILoggerExtensions
{
    public static Exception LogException<T>(this ILogger<T> logger, Exception ex, LogLevel logLevel = LogLevel.Error, params object?[] args)
    {
        logger.Log(logLevel, ex?.Message, args);
        return ex;
    }
    public static Exception LogException(this ILogger logger, Exception ex, LogLevel logLevel = LogLevel.Error, params object?[] args)
    {
        logger.Log(logLevel, ex?.Message, args);
        return ex;
    }
}
