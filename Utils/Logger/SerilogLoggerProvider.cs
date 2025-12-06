namespace Utils.Analytics
{
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Core;
    using Serilog.Sinks.SystemConsole.Themes;
    using System;

    /// <summary>
    /// Провайдер логгирования на основе Serilog.
    /// </summary>
    public class SerilogLoggerProvider : ILoggerProvider
	{
		private readonly Logger _logger;
		private bool _disposed = false;

		public SerilogLoggerProvider()
		{
#if DEBUG
			// Для отладки создаем консоль
			NativeMethods.AllocConsole();

			_logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.Console(
					outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
					theme: AnsiConsoleTheme.Code)
				.CreateLogger();
#else
            // Для релиза можно настроить другие sinks (файл, EventLog и т.д.)
            _logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug() // Или другие sinks
                .CreateLogger();
#endif
		}

		public SerilogLoggerProvider(LoggerConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			_logger = configuration.CreateLogger();
		}

		public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
		{
			return new SerilogLogger(_logger, categoryName);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_logger?.Dispose();
				}
				_disposed = true;
			}
		}
	}
}
