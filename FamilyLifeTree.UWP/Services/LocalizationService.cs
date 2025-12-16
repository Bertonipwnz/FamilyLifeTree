namespace FamilyLifeTree.UWP.Services
{
	using FamilyLifeTree.Core.Interfaces;
	using Serilog;
	using System;
	using Utils.Logger;
	using Windows.ApplicationModel.Resources;

	/// <summary>
	/// Сервис локализации.
	/// </summary>
	public class LocalizationService : ILocalizationService
	{
		#region Private Fields

		/// <summary>
		/// Дефолтный ресурс лоадер (Resources.resw)
		/// </summary>
		private static readonly ResourceLoader _defaultResourceLoader = ResourceLoader.GetForCurrentView();

		/// <summary>
		/// Логгер.
		/// </summary>
		private static readonly ILogger _logger = LogService.GetCurrentLogger();

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public string GetLocalizationString(string resourcesKey, string? resourcesName = null)
		{
			try
			{
				ResourceLoader resourceLoader = resourcesName == null ? _defaultResourceLoader : ResourceLoader.GetForCurrentView(resourcesName) ?? _defaultResourceLoader;

				return resourceLoader.GetString(resourcesKey);
			}
			catch (Exception ex)
			{
				_logger?.Error(ex.Message ?? ex.ToString());
				return string.Empty;
			}
		}

		#endregion
	}
}
