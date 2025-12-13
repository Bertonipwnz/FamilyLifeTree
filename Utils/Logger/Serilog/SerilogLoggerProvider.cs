namespace Utils.Logger
{
	using Microsoft.Extensions.Logging;
	using Serilog;
	using Serilog.Core;
	using Serilog.Sinks.SystemConsole.Themes;
	using System;

	/// <summary>
	/// Провайдер логгеров, реализующий интеграцию Serilog
	/// с системой логгирования Microsoft.Extensions.Logging.
	/// </summary>
	public class SerilogLoggerProvider : ILoggerProvider
	{
		#region Private Fields

		/// <summary>
		/// Экземпляр Serilog <see cref="Logger"/>.
		/// </summary>
		private readonly Logger _logger;

		/// <summary>
		/// Флаг, указывающий, был ли объект уже освобождён.
		/// </summary>
		private bool _disposed = false;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Создаёт новый провайдер логгирования Serilog 
		/// с конфигурацией по умолчанию (для DEBUG — консоль, для RELEASE — Debug sink).
		/// </summary>
		public SerilogLoggerProvider()
		{
#if DEBUG
			NativeMethods.AllocConsole();

			_logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.Console(
					outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
					theme: AnsiConsoleTheme.Code)
				.CreateLogger();
#else
			_logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Debug()
				.CreateLogger();
#endif
		}

		/// <summary>
		/// Создаёт провайдер логгирования с пользовательской конфигурацией Serilog.
		/// </summary>
		/// <param name="configuration">Объект конфигурации Serilog.</param>
		/// <exception cref="ArgumentNullException">Если <paramref name="configuration"/> равен null.</exception>
		public SerilogLoggerProvider(LoggerConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			_logger = configuration.CreateLogger();
		}

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public Microsoft.Extensions.Logging.ILogger CreateLogger(string categoryName)
		{
			return new SerilogLogger(_logger, categoryName);
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Освобождает ресурсы, занятые логгером.
		/// </summary>
		/// <param name="disposing">Признак явного вызова <see cref="Dispose()"/>.</param>
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

		#endregion
	}
}
