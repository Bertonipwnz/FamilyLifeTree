namespace FamilyLifeTree.UWP.Helpers
{
	using FamilyLifeTree.Core.Interfaces;
	using Windows.ApplicationModel;
	using Windows.Storage;

	/// <summary>
	/// Хелпер по работе с путями.
	/// </summary>
	public class PathHelper : IPathHelper
	{
		#region Public Methods

		/// <summary>
		/// Получает путь к папке установки.
		/// </summary>
		public string GetInstalledFolderPath()
		{
			var folder = Package.Current.InstalledLocation;
			return folder.Path ?? string.Empty;
		}

		/// <summary>
		/// Получает путь к локальной папке.
		/// </summary>
		public string GetLocalFolderPath()
		{
			var folder = ApplicationData.Current.LocalFolder;
			return folder.Path ?? string.Empty;
		}

		#endregion
	}
}
