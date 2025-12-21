namespace FamilyLifeTree.ViewModels.Entities
{
	using FamilyLifeTree.Core.Models;
	using Utils.Mvvm.ViewModels;

	/// <summary>
	/// Модель представления гендера.
	/// </summary>
	public class GenderViewModel : AbstractEntityViewModel<GenderModel>
	{
		#region Private Fields

		private readonly GenderModel _model;

		#endregion

		#region Public Properties

		public string IconPath => _model.IconPath;

		#endregion

		#region Public Constructors

		/// <summary>
		/// Создает экземпляр <see cref="GenderViewModel"/>
		/// </summary>
		/// <param name="model">Модель.</param>
		public GenderViewModel(GenderModel model) : base(model)
		{
			_model = model;
		}

		#endregion
	}
}
