namespace FamilyLifeTree.Services
{
	using FamilyLifeTree.Core.Interfaces;
	using FamilyLifeTree.Core.Models;
	using FamilyLifeTree.ViewModels.Entities;
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Utils.Interfaces;

	/// <summary>
	/// Сервис гендеров.
	/// </summary>
	public class GenderService : IGenderService<GenderModel, GenderViewModel>
	{
		#region Public Properties

		/// <inheritdoc/>
		public IEnumerable<GenderViewModel> ViewModels { get; private set; }

		/// <inheritdoc/>
		public bool IsInitialized { get; private set; }

		/// <inheritdoc/>
		public event EventHandler Initialized;

		#endregion Public Properties

		/// <inheritdoc/>
		GenderViewModel IEntityService<GenderModel, GenderViewModel>.CreateVM(GenderModel model)
		{
			throw new NotImplementedException();
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
