namespace FamilyLifeTree.ViewModels.Entities
{
	using FamilyLifeTree.Core.Models;
	using Utils.Mvvm.ViewModels;

	/// <summary>
	/// Модель представления гендера.
	/// </summary>
	public class GenderViewModel : AbstractEntityViewModel<GenderModel>
	{
		#region Public Constructors

		/// <summary>
		/// Создает экземпляр <see cref="GenderViewModel"/>
		/// </summary>
		/// <param name="model">Модель.</param>
		public GenderViewModel(GenderModel model) : base(model)
		{
		}

		#endregion
	}
}
