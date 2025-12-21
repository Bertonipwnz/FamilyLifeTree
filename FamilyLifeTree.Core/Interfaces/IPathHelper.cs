namespace FamilyLifeTree.Core.Interfaces
{
	/// <summary>
	/// Хелпер по работе с путям.
	/// </summary>
	public interface IPathHelper
	{
		/// <summary>
		/// Получает путь к папке установки.
		/// </summary>
		public string GetInstalledFolderPath();

		/// <summary>
		/// Получает путь к локальной папке.
		/// </summary>
		public string GetLocalFolderPath();
	}
}
