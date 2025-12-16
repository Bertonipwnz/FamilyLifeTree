namespace FamilyLifeTree.UWP.Services
{
	using FamilyLifeTree.Core.Interfaces;
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

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public string GetLocalizationString(string resourcesKey, string? resourcesName = null)
		{
			var loader = resourcesName == null
					? _defaultResourceLoader
					: ResourceLoader.GetForCurrentView(resourcesName) ?? _defaultResourceLoader;

			return loader.GetString(resourcesKey) ?? string.Empty;
		}

		#endregion
	}
}
