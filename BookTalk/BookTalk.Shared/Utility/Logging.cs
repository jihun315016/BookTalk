using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTalk.Shared.Utility;

public class Logging
{
    public static void WriteError<T>(ILogger<T> logger, Exception ex)
    {
        logger.LogError(ex, "");
    }

    public static void WriteError<T>(ILogger<T> logger)
    {
        logger.LogError("");
    }
}
