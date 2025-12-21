namespace FamilyLifeTree.Services
{
	using FamilyLifeTree.Core.Interfaces;
	using FamilyLifeTree.Core.Models;
	using FamilyLifeTree.ViewModels.Entities;
	using Serilog;
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Utils.Interfaces;
	using Utils.Logger;
	using Utils.Serialization.Services.Interfaces;

#nullable enable

	//TODO: Можно сделать базовый абстрактный класс который реализует некоторую логику сервиса и передавать просто параметры.
	/// <summary>
	/// Сервис гендеров.
	/// </summary>
	public class GenderService : IEntityService<GenderModel, GenderViewModel>, IAsyncInitializable
	{
		#region Private Fields

		/// <summary>
		/// Хелпер по работе с путями.
		/// </summary>
		private readonly IPathHelper _pathHelper;

		/// <summary>
		/// Сервис по работе с файлами.
		/// </summary>
		private readonly IFileService _fileService;

		/// <summary>
		/// Сервис по работе с JSON.
		/// </summary>
		private readonly IJsonSerializationService _jsonService;

		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger? _logger = LogService.GetCurrentLogger();

		#endregion

		#region Public Properties

		/// <inheritdoc/>
		public IEnumerable<GenderViewModel> ViewModels { get; private set; } = new List<GenderViewModel>();

		/// <inheritdoc/>
		public bool IsInitialized { get; private set; }

		/// <inheritdoc/>
		public event EventHandler? Initialized;

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Создает экземпляр <see cref="GenderService"/>
		/// </summary>
		public GenderService(IFileService fileService, IJsonSerializationService jsonService, IPathHelper pathHelper)
		{
			_jsonService = jsonService;
			_fileService = fileService;
			_pathHelper = pathHelper;
		}

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public async Task InitializeAsync(CancellationToken cancellationToken = default)
		{
			if (IsInitialized)
				return;

			string relativePath = Path.Combine("Assets", "Genders", "GendersData.json");
			string fileContent = await _fileService.ReadAllTextFromInstalledPathAsync(relativePath);

			if (string.IsNullOrWhiteSpace(fileContent))
			{
				_logger?.Warning("Файл с данными о гендерах пуст или не найден: {Path}", relativePath);
				IsInitialized = true;
				Initialized?.Invoke(this, EventArgs.Empty);
				return;
			}

			List<GenderModel> models = _jsonService.Deserialize<List<GenderModel>>(fileContent) ?? new List<GenderModel>();
			models.ForEach(x => x.IconPath = Path.Combine(_pathHelper.GetInstalledFolderPath(), x.IconPath));
			ViewModels = models.Select(x => new GenderViewModel(x));

			IsInitialized = true;
			Initialized?.Invoke(this, EventArgs.Empty);
		}

		#endregion
	}
}
