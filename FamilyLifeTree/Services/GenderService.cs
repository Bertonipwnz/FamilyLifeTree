namespace FamilyLifeTree.Services
{
	using FamilyLifeTree.Core.Models;
	using FamilyLifeTree.ViewModels.Entities;
	using Serilog;
	using System;
	using System.Collections.Generic;
	using System.IO;
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
		private readonly IFileService _fileService;
		private readonly IJsonSerializationService _jsonService;
		private readonly ILogger _logger;

		#region Public Properties

		/// <inheritdoc/>
		public IEnumerable<GenderViewModel> ViewModels { get; private set; }

		/// <inheritdoc/>
		public bool IsInitialized { get; private set; }

		/// <inheritdoc/>
		public event EventHandler Initialized;

		#endregion Public Properties

		public GenderService(IFileService fileService, IJsonSerializationService jsonService, ILogger logger)
		{
			_logger = logger;
			_jsonService = jsonService;
			_fileService = fileService;
		}

		/// <inheritdoc/>
		GenderViewModel IEntityService<GenderModel, GenderViewModel>.CreateVM(GenderModel model)
		{
			return new GenderViewModel(model);
		}

		/// <inheritdoc/>
		public async Task InitializeAsync(CancellationToken cancellationToken = default)
		{
			string relativePath = Path.Combine("Assets", "Genders", "GendersData.json");
			string fileContent = await _fileService.ReadAllTextFromInstalledPathAsync(relativePath);

			if (string.IsNullOrWhiteSpace(fileContent))
			{
				//TODO: Логгирование.
				return;
			}

			IEnumerable<GenderModel> models = _jsonService.Deserialize<IEnumerable<GenderModel>>(fileContent) ?? new List<GenderModel>();

			IsInitialized = true;
			Initialized?.Invoke(this, EventArgs.Empty);
		}
	}
}
