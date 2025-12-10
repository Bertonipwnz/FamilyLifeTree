namespace FamilyLifeTree.ViewModels.Entities
{
	using FamilyLifeTree.Core.Models;
	using Utils.Mvvm.ViewModels;

	/// <summary>
	/// Модель человека.
	/// </summary>
	public class PersonViewModel : AbstractEntityViewModel<Person>
	{
		/// <summary>
		/// Создает экземпляр <see cref="PersonViewModel"/>
		/// </summary>
		/// <param name="model">Модель.</param>
		public PersonViewModel(Person model) : base(model)
		{
		}
	}
}
