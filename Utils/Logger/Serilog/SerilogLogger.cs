namespace Utils.Logger
{
	using Microsoft.Extensions.Logging;
	using System;

	/// <summary>
	/// Реализация <see cref="Microsoft.Extensions.Logging.ILogger"/>
	/// на основе Serilog.
	/// Адаптирует уровни логов и форматирование сообщений под Serilog.
	/// </summary>
	public class SerilogLogger : Microsoft.Extensions.Logging.ILogger
	{
		#region Private Fields

		/// <summary>
		/// Экземпляр логгера Serilog.
		/// </summary>
		private readonly Serilog.ILogger _logger;

		/// <summary>
		/// Имя категории, переданное из инфраструктуры логгирования Microsoft.
		/// </summary>
		private readonly string _categoryName;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Создаёт новый экземпляр <see cref="SerilogLogger"/>.
		/// </summary>
		/// <param name="logger">Экземпляр Serilog логгера.</param>
		/// <param name="categoryName">Имя категории логирования.</param>
		public SerilogLogger(Serilog.ILogger logger, string categoryName)
		{
			_logger = logger.ForContext("SourceContext", categoryName);
			_categoryName = categoryName;
		}

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public IDisposable BeginScope<TState>(TState state)
		{
			return Serilog.Context.LogContext.PushProperty("Scope", state);
		}

		/// <inheritdoc/>
		public bool IsEnabled(LogLevel logLevel)
		{
			return _logger.IsEnabled(ConvertLogLevel(logLevel));
		}

		/// <inheritdoc/>
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

		#endregion

		#region Private Methods

		/// <summary>
		/// Преобразует уровень логирования Microsoft в уровень Serilog.
		/// </summary>
		/// <param name="logLevel">Уровень логирования Microsoft.</param>
		/// <returns>Соответствующий уровень Serilog.</returns>
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

		#endregion
	}
}
