namespace FamilyLifeTree.Services
{
	using FamilyLifeTree.Core.Models;
	using FamilyLifeTree.ViewModels.Entities;
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Utils.Interfaces;
	using Utils.Serialization.Services.Interfaces;

	/// <summary>
	/// Сервис гендеров.
	/// </summary>
	public class GenderService : IEntityService<GenderModel, GenderViewModel>
	{
		private readonly IFileService _fileService;
		private readonly IJsonSerializationService _jsonService;

		#region Public Properties

		/// <inheritdoc/>
		public IEnumerable<GenderViewModel> ViewModels { get; private set; }

		/// <inheritdoc/>
		public bool IsInitialized { get; private set; }

		/// <inheritdoc/>
		public event EventHandler Initialized;

		#endregion Public Properties

		public GenderService(IFileService fileService, IJsonSerializationService jsonService)
		{
			_jsonService = jsonService;
			_fileService = fileService;
		}

		/// <inheritdoc/>
		GenderViewModel IEntityService<GenderModel, GenderViewModel>.CreateVM(GenderModel model)
		{
			return new GenderViewModel(model);
		}

		/// <inheritdoc/>
		public Task InitializeAsync(CancellationToken cancellationToken = default)
		{
			throw new NotImplementedException();

			IsInitialized = true;
			Initialized?.Invoke(this, EventArgs.Empty);
		}
	}
}
