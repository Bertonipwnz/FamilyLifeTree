namespace Utils.Logger
{
	using Serilog;
	using Serilog.Sinks.SystemConsole.Themes;
	using System;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Содержит P/Invoke вызовы к нативным функциям Windows.
	/// </summary>
	internal static class NativeMethods
	{
		#region Public Methods

		/// <summary>
		/// Создаёт консольное окно для текущего процесса.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> если консоль успешно создана; 
		/// иначе <see langword="false"/>.
		/// </returns>
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AllocConsole();

		#endregion Public Methods
	}

	/// <summary>
	/// Предоставляет статический сервис для конфигурации и получения логгера Serilog.
	/// Используется для обратной совместимости и быстрых сценариев логгирования.
	/// </summary>
	public static class LogService
	{
		#region Private Fields

		/// <summary>
		/// Текущий экземпляр логгера Serilog.
		/// </summary>
		private static Serilog.ILogger _logger;

		#endregion

		#region Constructors

		/// <summary>
		/// Статический конструктор. Выполняет инициализацию базового логгера.
		/// </summary>
		static LogService()
		{
#if DEBUG
			NativeMethods.AllocConsole();

			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Verbose()
				.WriteTo.Console(theme: AnsiConsoleTheme.Code)
				.CreateLogger();
#else
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Information()
				.WriteTo.Debug()
				.CreateLogger();
#endif

			_logger = Log.Logger;
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Возвращает текущий экземпляр логгера Serilog.
		/// </summary>
		/// <returns>Объект <see cref="Serilog.ILogger"/>.</returns>
		public static Serilog.ILogger GetCurrentLogger() => _logger ?? Log.Logger;

		/// <summary>
		/// Позволяет переопределить или дополнительно сконфигурировать логгер Serilog.
		/// </summary>
		/// <param name="configure">
		/// Делегат конфигурации <see cref="LoggerConfiguration"/>.
		/// Если передан <see langword="null"/>, используется конфигурация по умолчанию.
		/// </param>
		public static void ConfigureLogger(Action<LoggerConfiguration> configure = null)
		{
			var config = new LoggerConfiguration();

#if DEBUG
			NativeMethods.AllocConsole();
			config.MinimumLevel.Verbose()
				  .WriteTo.Console(theme: AnsiConsoleTheme.Code);
#else
			config.MinimumLevel.Information()
				  .WriteTo.Debug();
#endif

			configure?.Invoke(config);

			Log.Logger = config.CreateLogger();
			_logger = Log.Logger;
		}

		#endregion
	}
}
