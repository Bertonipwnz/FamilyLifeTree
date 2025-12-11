namespace FamilyLifeTree.ViewModels.Entities
{
	using FamilyLifeTree.Core.Models;
	using Utils.Interfaces;
	using Utils.Mvvm.ViewModels;

	/// <summary>
	/// Модель представления человека для отображения на Canvas.
	/// </summary>
	public class PersonViewModel : AbstractEntityViewModel<Person>, IMoveable
	{
		/// <summary>
		/// Модель домена.
		/// </summary>
		private readonly Person _person;

		/// <summary>
		/// Координата X на Canvas.
		/// </summary>
		private double _x;

		/// <summary>
		/// Координата Y на Canvas.
		/// </summary>
		private double _y;

		/// <summary>
		/// Отображаемое имя.
		/// </summary>
		public string DisplayName => _person.ShortName;

		public double X
		{
			get => _x;
			set => SetProperty(ref _x, value);
		}

		public double Y
		{
			get => _y;
			set => SetProperty(ref _y, value);
		}

		/// <summary>
		/// Создает экземпляр <see cref="PersonViewModel"/>
		/// </summary>
		/// <param name="model">Модель.</param>
		public PersonViewModel(Person model) : base(model)
		{
			_person = model;
		}

		public Person GetModel() => _person;
	}
}
