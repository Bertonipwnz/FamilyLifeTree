namespace FamilyLifeTree.UWP.Services
{
	using FamilyLifeTree.Core.Interfaces;
	using Serilog;
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
		private readonly ILogger? _logger = LogService.GetCurrentLogger();

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public string GetLocalizationString(string resourcesKey, string? resourcesName = null)
		{
			var loader = resourcesName == null
					? _defaultResourceLoader
					: ResourceLoader.GetForCurrentView(resourcesName) ?? _defaultResourceLoader;

			_logger?.Debug($"Get string from key {resourcesKey} from {resourcesName}");

			return loader.GetString(resourcesKey) ?? string.Empty;
		}

		#endregion
	}
}
