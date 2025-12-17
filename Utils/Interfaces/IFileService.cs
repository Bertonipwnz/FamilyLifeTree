namespace Utils.Interfaces
{
	using System.IO;
	using System.Threading.Tasks;

#nullable enable

	/// <summary>
	/// Сервис работы с файлами в установочной и локальной папках приложения.
	/// Интерфейс платформо-независимый (работает через потоки и строки).
	/// </summary>
	public interface IFileService
	{
		/// <summary>
		/// Проверяет существование файла в установочной папке приложения.
		/// </summary>
		/// <param name="relativePath">Относительный путь к файлу от InstalledPath.</param>
		/// <returns><c>true</c>, если файл существует.</returns>
		Task<bool> ExistsInInstalledPathAsync(string relativePath);

		/// <summary>
		/// Проверяет существование файла в локальной папке приложения.
		/// </summary>
		/// <param name="relativePath">Относительный путь к файлу от LocalPath.</param>
		/// <returns><c>true</c>, если файл существует.</returns>
		Task<bool> ExistsInLocalPathAsync(string relativePath);

		/// <summary>
		/// Открывает файл из установочной папки для чтения.
		/// </summary>
		/// <param name="relativePath">Относительный путь к файлу от InstalledPath.</param>
		/// <returns>Поток для чтения.</returns>
		Task<Stream> OpenReadFromInstalledPathAsync(string relativePath);

		/// <summary>
		/// Открывает файл из локальной папки для чтения.
		/// </summary>
		/// <param name="relativePath">Относительный путь к файлу от LocalPath.</param>
		/// <returns>Поток для чтения.</returns>
		Task<Stream> OpenReadFromLocalPathAsync(string relativePath);

		/// <summary>
		/// Открывает или создаёт файл в локальной папке для чтения и записи.
		/// При необходимости создаёт недостающие подкаталоги.
		/// </summary>
		/// <param name="relativePath">Относительный путь к файлу от LocalPath.</param>
		/// <returns>Поток для чтения/записи.</returns>
		Task<Stream> OpenOrCreateLocalFileAsync(string relativePath);

		/// <summary>
		/// Считывает весь текст из файла в установочной папке.
		/// </summary>
		/// <param name="relativePath">Относительный путь к файлу от InstalledPath.</param>
		/// <returns>Содержимое файла.</returns>
		Task<string> ReadAllTextFromInstalledPathAsync(string relativePath);

		/// <summary>
		/// Считывает весь текст из файла в локальной папке.
		/// </summary>
		/// <param name="relativePath">Относительный путь к файлу от LocalPath.</param>
		/// <returns>Содержимое файла.</returns>
		Task<string> ReadAllTextFromLocalPathAsync(string relativePath);

		/// <summary>
		/// Записывает текст в файл в локальной папке, создавая его при необходимости.
		/// </summary>
		/// <param name="relativePath">Относительный путь к файлу от LocalPath.</param>
		/// <param name="contents">Содержимое файла.</param>
		Task WriteAllTextToLocalPathAsync(string relativePath, string contents);
	}
}


