namespace Utils.Analytics
{
    using Microsoft.Extensions.Logging;
    using System;

    /// <summary>
    /// Реализация ILogger на основе Serilog.
    /// </summary>
    public class SerilogLogger : Microsoft.Extensions.Logging.ILogger
	{
		private readonly Serilog.ILogger _logger;
		private readonly string _categoryName;

		public SerilogLogger(Serilog.ILogger logger, string categoryName)
		{
			_logger = logger.ForContext("SourceContext", categoryName);
			_categoryName = categoryName;
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return Serilog.Context.LogContext.PushProperty("Scope", state);
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return _logger.IsEnabled(ConvertLogLevel(logLevel));
		}

		public void Log<TState>(
			LogLevel logLevel,
			EventId eventId,
			TState state,
			Exception exception,
			Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
				return;

			var message = formatter(state, exception);
			var serilogLevel = ConvertLogLevel(logLevel);

			_logger.Write(serilogLevel, exception, "[{EventId}] {Message}", eventId, message);
		}

		private static Serilog.Events.LogEventLevel ConvertLogLevel(LogLevel logLevel)
		{
			return logLevel switch
			{
				LogLevel.Trace => Serilog.Events.LogEventLevel.Verbose,
				LogLevel.Debug => Serilog.Events.LogEventLevel.Debug,
				LogLevel.Information => Serilog.Events.LogEventLevel.Information,
				LogLevel.Warning => Serilog.Events.LogEventLevel.Warning,
				LogLevel.Error => Serilog.Events.LogEventLevel.Error,
				LogLevel.Critical => Serilog.Events.LogEventLevel.Fatal,
				LogLevel.None => Serilog.Events.LogEventLevel.Fatal,
				_ => Serilog.Events.LogEventLevel.Information
			};
		}
	}

}
