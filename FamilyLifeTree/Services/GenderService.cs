namespace FamilyLifeTree.Services
{
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
	using Utils.Serialization.Services.Interfaces;

	//TODO: Можно сделать базовый абстрактный класс который реализует некоторую логику сервиса и передавать просто параметры.
	/// <summary>
	/// Сервис гендеров.
	/// </summary>
	public class GenderService : IEntityService<GenderModel, GenderViewModel>
	{
		#region Private Fields

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
		private readonly ILogger _logger;

		#endregion

		#region Public Properties

		/// <inheritdoc/>
		public IEnumerable<GenderViewModel> ViewModels { get; private set; } = new List<GenderViewModel>();

		/// <inheritdoc/>
		public bool IsInitialized { get; private set; }

		/// <inheritdoc/>
		public event EventHandler Initialized;

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Создает экземпляр <see cref="GenderService"/>
		/// </summary>
		public GenderService(IFileService fileService, IJsonSerializationService jsonService, ILogger logger)
		{
			_logger = logger;
			_jsonService = jsonService;
			_fileService = fileService;
		}

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public async Task InitializeAsync(CancellationToken cancellationToken = default)
		{
			string relativePath = Path.Combine("Assets", "Genders", "GendersData.json");
			string fileContent = await _fileService.ReadAllTextFromInstalledPathAsync(relativePath);

			if (string.IsNullOrWhiteSpace(fileContent))
			{
				_logger.Warning("Файл с данными о гендерах пуст или не найден: {Path}", relativePath);
				IsInitialized = true;
				Initialized?.Invoke(this, EventArgs.Empty);
				return;
			}

			IEnumerable<GenderModel> models = _jsonService.Deserialize<IEnumerable<GenderModel>>(fileContent) ?? new List<GenderModel>();
			ViewModels = models.Select(x => new GenderViewModel(x));

			IsInitialized = true;
			Initialized?.Invoke(this, EventArgs.Empty);
		}

		#endregion
	}
}
