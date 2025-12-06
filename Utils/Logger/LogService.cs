namespace Utils.Analytics
{
    using Serilog;
    using Serilog.Sinks.SystemConsole.Themes;
    using System;
    using System.Runtime.InteropServices;


    /// <summary>
    /// Нативные методы.
    /// </summary>
	internal static class NativeMethods
	{
		#region Public Methods

		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AllocConsole();

		#endregion Public Methods
	}

	/// <summary>
	/// Сервис логгирования (для обратной совместимости).
	/// </summary>
	public static class LogService
	{
		private static Serilog.ILogger _logger;

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

		public static Serilog.ILogger GetCurrentLogger() => _logger ?? Log.Logger;

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
	}
}
