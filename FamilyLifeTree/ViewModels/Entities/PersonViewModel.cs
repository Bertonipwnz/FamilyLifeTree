namespace FamilyLifeTree.ViewModels.Entities
{
    using FamilyLifeTree.Core.Models;
    using Utils.Mvvm.ViewModels;

    /// <summary>
    /// Модель представления человека для отображения на Canvas.
    /// </summary>
    public class PersonViewModel : AbstractEntityViewModel<Person>
	{
		/// <summary>
		/// Модель домена.
		/// </summary>
		private readonly Person _person;

		/// <summary>
		/// Координата X на Canvas.
		/// </summary>
		private double _canvasLeft;

		/// <summary>
		/// Координата Y на Canvas.
		/// </summary>
		private double _canvasTop;

		/// <summary>
		/// Создает экземпляр <see cref="PersonViewModel"/>
		/// </summary>
		/// <param name="model">Модель.</param>
		public PersonViewModel(Person model) : base(model)
		{
			_person = model;
		}

		/// <summary>
		/// Доменная модель (используется для операций репозитория).
		/// </summary>
		public Person Model => _person;

		/// <summary>
		/// Отображаемое имя.
		/// </summary>
		public string DisplayName => _person.ShortName;

		/// <summary>
		/// Позиция элемента по оси X.
		/// </summary>
		public double CanvasLeft
		{
			get => _canvasLeft;
			set => SetProperty(ref _canvasLeft, value);
		}

		/// <summary>
		/// Позиция элемента по оси Y.
		/// </summary>
		public double CanvasTop
		{
			get => _canvasTop;
			set => SetProperty(ref _canvasTop, value);
		}
	}
}
