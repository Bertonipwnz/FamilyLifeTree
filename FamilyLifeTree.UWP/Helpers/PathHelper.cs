namespace FamilyLifeTree.UWP.Helpers
{
    using FamilyLifeTree.Core.Interfaces;
    using Windows.ApplicationModel;
    using Windows.Storage;

    public class PathHelper : IPathHelper
	{
		public string GetInstalledFolderPath()
		{
			var folder = Package.Current.InstalledLocation;
			return folder.Path ?? string.Empty;
		}

		public string GetLocalFolderPath()
		{
			var folder = ApplicationData.Current.LocalFolder;
			return folder.Path ?? string.Empty;
		}
	}
}
