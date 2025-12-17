namespace FamilyLifeTree.UWP.Services
{
	using Serilog;
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using Utils.Interfaces;
	using Utils.Logger;
	using Windows.ApplicationModel;
	using Windows.Storage;

#nullable enable

	/// <summary>
	/// UWP-реализация <see cref="IFileService"/> с использованием InstalledPath и LocalPath.
	/// </summary>
	public class FileService : IFileService
	{
		#region Private Fields

		/// <summary>
		/// Логгер текущего класса.
		/// </summary>
		private readonly ILogger _logger = LogService.GetCurrentLogger();

		#endregion

		#region Public Methods

		/// <inheritdoc />
		public async Task<bool> ExistsInInstalledPathAsync(string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
			{
				_logger?.Error("Path must not be empty.", nameof(ExistsInInstalledPathAsync));
				throw new ArgumentException("Path must not be empty.", nameof(relativePath));
			}

			try
			{
				var folder = Package.Current.InstalledLocation;
				_ = await folder.GetFileAsync(relativePath.Replace(Path.DirectorySeparatorChar, '\\'));

				return true;
			}
			catch (FileNotFoundException fileException)
			{
				_logger?.Error(fileException.Message ?? fileException.ToString());

				return false;
			}
			catch (Exception ex)
			{
				_logger?.Error(ex.Message ?? ex.ToString());

				return false;
			}
		}

		/// <inheritdoc />
		public async Task<bool> ExistsInLocalPathAsync(string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
			{
				_logger?.Error("Path must not be empty.", nameof(ExistsInLocalPathAsync));
				throw new ArgumentException("Path must not be empty.", nameof(relativePath));
			}

			try
			{
				var folder = ApplicationData.Current.LocalFolder;
				_ = await folder.GetFileAsync(relativePath.Replace(Path.DirectorySeparatorChar, '\\'));

				return true;
			}
			catch (FileNotFoundException fileException)
			{
				_logger?.Error(fileException.Message ?? fileException.ToString());

				return false;
			}
			catch (Exception ex)
			{
				_logger?.Error(ex.Message ?? ex.ToString());

				return false;
			}
		}

		/// <inheritdoc />
		public async Task<Stream> OpenReadFromInstalledPathAsync(string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
			{
				_logger?.Error("Path must not be empty.", nameof(OpenReadFromInstalledPathAsync));
				throw new ArgumentException("Path must not be empty.", nameof(relativePath));
			}

			var folder = Package.Current.InstalledLocation;
			var file = await folder
				.GetFileAsync(relativePath.Replace(Path.DirectorySeparatorChar, '\\'));

			return await file.OpenStreamForReadAsync();
		}

		/// <inheritdoc />
		public async Task<Stream> OpenReadFromLocalPathAsync(string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
			{
				_logger?.Error("Path must not be empty.", nameof(OpenReadFromLocalPathAsync));
				throw new ArgumentException("Path must not be empty.", nameof(relativePath));
			}

			var folder = ApplicationData.Current.LocalFolder;
			var file = await folder
				.GetFileAsync(relativePath.Replace(Path.DirectorySeparatorChar, '\\'));

			return await file.OpenStreamForReadAsync();
		}

		/// <inheritdoc />
		public async Task<Stream> OpenOrCreateLocalFileAsync(string relativePath)
		{
			if (string.IsNullOrWhiteSpace(relativePath))
			{
				_logger?.Error("Path must not be empty.", nameof(OpenOrCreateLocalFileAsync));
				throw new ArgumentException("Path must not be empty.", nameof(relativePath));
			}

			var localFolder = ApplicationData.Current.LocalFolder;

			// Поддержка относительных путей с подкаталогами: "folder1/folder2/file.json"
			var normalized = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			var segments = normalized.Split(new[] { Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

			if (segments.Length == 0)
			{
				_logger?.Error("Path must contain file name.", nameof(OpenOrCreateLocalFileAsync));
				throw new ArgumentException("Path must contain file name.", nameof(relativePath));
			}

			var fileName = segments.Last();
			var currentFolder = localFolder;

			// Создаём все промежуточные каталоги
			for (var i = 0; i < segments.Length - 1; i++)
			{
				currentFolder = await currentFolder
					.CreateFolderAsync(segments[i], CreationCollisionOption.OpenIfExists);
			}

			var file = await currentFolder
				.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

			// Открываем поток с возможностью чтения и записи
			return await file.OpenStreamForWriteAsync();
		}

		/// <inheritdoc />
		public async Task<string> ReadAllTextFromInstalledPathAsync(string relativePath)
		{
			using (var stream = await OpenReadFromInstalledPathAsync(relativePath))
			{
				using (var reader = new StreamReader(stream))
				{
					return await reader.ReadToEndAsync();
				}
			}
		}

		/// <inheritdoc />
		public async Task<string> ReadAllTextFromLocalPathAsync(string relativePath)
		{
			using (var stream = await OpenReadFromLocalPathAsync(relativePath))
			{
				using var reader = new StreamReader(stream);
				return await reader.ReadToEndAsync();
			}
		}

		/// <inheritdoc />
		public async Task WriteAllTextToLocalPathAsync(string relativePath, string contents)
		{
			using (var stream = await OpenOrCreateLocalFileAsync(relativePath))
			{
				stream.SetLength(0);

				using (var writer = new StreamWriter(stream))
				{
					await writer.WriteAsync(contents);
					await writer.FlushAsync();
				}
			}
		}

		#endregion
	}
}


