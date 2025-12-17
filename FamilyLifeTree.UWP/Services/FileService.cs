namespace FamilyLifeTree.UWP.Services
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using Utils.Interfaces;
	using Windows.ApplicationModel;
	using Windows.Storage;

#nullable enable

	/// <summary>
	/// UWP-реализация <see cref="IFileService"/> с использованием InstalledPath и LocalPath.
	/// </summary>
	public class FileService : IFileService
	{
		/// <inheritdoc />
		public async Task<bool> ExistsInInstalledPathAsync(string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
			{
				throw new ArgumentException("Path must not be empty.", nameof(relativePath));
			}

			try
			{
				var folder = Package.Current.InstalledLocation;
				_ = await folder.GetFileAsync(relativePath.Replace(Path.DirectorySeparatorChar, '\\'))
					.AsTask().ConfigureAwait(false);

				return true;
			}
			catch (FileNotFoundException)
			{
				return false;
			}
			catch (Exception)
			{
				// Для любых других ошибок считаем, что файла нет.
				return false;
			}
		}

		/// <inheritdoc />
		public async Task<bool> ExistsInLocalPathAsync(string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
			{
				throw new ArgumentException("Path must not be empty.", nameof(relativePath));
			}

			try
			{
				var folder = ApplicationData.Current.LocalFolder;
				_ = await folder.GetFileAsync(relativePath.Replace(Path.DirectorySeparatorChar, '\\'))
					.AsTask().ConfigureAwait(false);

				return true;
			}
			catch (FileNotFoundException)
			{
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <inheritdoc />
		public async Task<Stream> OpenReadFromInstalledPathAsync(string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
			{
				throw new ArgumentException("Path must not be empty.", nameof(relativePath));
			}

			var folder = Package.Current.InstalledLocation;
			var file = await folder
				.GetFileAsync(relativePath.Replace(Path.DirectorySeparatorChar, '\\'))
				.AsTask().ConfigureAwait(false);

			return await file.OpenStreamForReadAsync().ConfigureAwait(false);
		}

		/// <inheritdoc />
		public async Task<Stream> OpenReadFromLocalPathAsync(string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
			{
				throw new ArgumentException("Path must not be empty.", nameof(relativePath));
			}

			var folder = ApplicationData.Current.LocalFolder;
			var file = await folder
				.GetFileAsync(relativePath.Replace(Path.DirectorySeparatorChar, '\\'))
				.AsTask().ConfigureAwait(false);

			return await file.OpenStreamForReadAsync().ConfigureAwait(false);
		}

		/// <inheritdoc />
		public async Task<Stream> OpenOrCreateLocalFileAsync(string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
			{
				throw new ArgumentException("Path must not be empty.", nameof(relativePath));
			}

			var localFolder = ApplicationData.Current.LocalFolder;

			// Поддержка относительных путей с подкаталогами: "folder1/folder2/file.json"
			var normalized = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			var segments = normalized.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			if (segments.Length == 0)
			{
				throw new ArgumentException("Path must contain file name.", nameof(relativePath));
			}

			var fileName = segments.Last();
			var currentFolder = localFolder;

			// Создаём все промежуточные каталоги
			for (var i = 0; i < segments.Length - 1; i++)
			{
				currentFolder = await currentFolder
					.CreateFolderAsync(segments[i], CreationCollisionOption.OpenIfExists)
					.AsTask().ConfigureAwait(false);
			}

			var file = await currentFolder
				.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists)
				.AsTask().ConfigureAwait(false);

			// Открываем поток с возможностью чтения и записи
			return await file.OpenStreamForWriteAsync().ConfigureAwait(false);
		}

		/// <inheritdoc />
		public async Task<string> ReadAllTextFromInstalledPathAsync(string relativePath)
		{
			using var stream = await OpenReadFromInstalledPathAsync(relativePath).ConfigureAwait(false);
			using var reader = new StreamReader(stream);
			return await reader.ReadToEndAsync().ConfigureAwait(false);
		}

		/// <inheritdoc />
		public async Task<string> ReadAllTextFromLocalPathAsync(string relativePath)
		{
			using var stream = await OpenReadFromLocalPathAsync(relativePath).ConfigureAwait(false);
			using var reader = new StreamReader(stream);
			return await reader.ReadToEndAsync().ConfigureAwait(false);
		}

		/// <inheritdoc />
		public async Task WriteAllTextToLocalPathAsync(string relativePath, string contents)
		{
			// Создаём/открываем файл и перезаписываем содержимое.
			using var stream = await OpenOrCreateLocalFileAsync(relativePath).ConfigureAwait(false);
			stream.SetLength(0); // очищаем файл

			using var writer = new StreamWriter(stream);
			await writer.WriteAsync(contents).ConfigureAwait(false);
			await writer.FlushAsync().ConfigureAwait(false);
		}
	}
}


