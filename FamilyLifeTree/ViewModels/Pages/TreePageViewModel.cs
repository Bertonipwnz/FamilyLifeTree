namespace FamilyLifeTree.ViewModels.Pages
{
	using CommunityToolkit.Mvvm.Input;
	using FamilyLifeTree.Core.Interfaces;
	using FamilyLifeTree.Core.Models;
	using FamilyLifeTree.ViewModels.Entities;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Threading.Tasks;
	using Utils.Mvvm.ViewModels;

	/// <summary>
	/// Модель представления страницы древа.
	/// </summary>
	public class TreePageViewModel : BasePageViewModel
	{
		/// <summary>
		/// Отступ между карточками на Canvas.
		/// </summary>
		private const double NodeMargin = 12d;

		/// <summary>
		/// Ширина карточки.
		/// </summary>
		private const double NodeWidth = 120d;

		/// <summary>
		/// Высота карточки.
		/// </summary>
		private const double NodeHeight = 64d;

		/// <summary>
		/// Unit of Work для работы с БД.
		/// </summary>
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Текущий счётчик созданных персон.
		/// </summary>
		private int _personCounter = 1;

		/// <summary>
		/// Коллекция людей, отображаемых на Canvas.
		/// </summary>
		public ObservableCollection<PersonViewModel> Persons { get; } = new();

		/// <summary>
		/// Команда добавления человека.
		/// </summary>
		public IAsyncRelayCommand AddPersonCommand { get; }

		/// <summary>
		/// Команда удаления человека.
		/// </summary>
		public IAsyncRelayCommand<PersonViewModel> RemovePersonCommand { get; }

		/// <summary>
		/// Создает экземпляр <see cref="TreePageViewModel"/>
		/// </summary>
		public TreePageViewModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

			AddPersonCommand = new AsyncRelayCommand(AddPersonAsync);
			RemovePersonCommand = new AsyncRelayCommand<PersonViewModel>(RemovePersonAsync);
		}

		/// <inheritdoc/>
		public override void OnNavigatedTo(object param = null)
		{
			_ = LoadPersonsAsync();
		}

		private async Task LoadPersonsAsync()
		{
			var persons = await _unitOfWork.Persons.GetAllAsync();

			Persons.Clear();

			int index = 0;
			foreach (var person in persons)
			{
				var vm = CreateVm(person, index);
				Persons.Add(vm);
				index++;
			}

			_personCounter = Persons.Count + 1;
		}

		private async Task AddPersonAsync()
		{
			var person = new Person
			{
				FirstName = "Person",
				LastName = _personCounter.ToString()
			};

			await _unitOfWork.Persons.AddAsync(person);
			await _unitOfWork.CompleteAsync();

			var vm = CreateVm(person, Persons.Count);
			Persons.Add(vm);

			_personCounter++;
		}

		private async Task RemovePersonAsync(PersonViewModel personViewModel)
		{
			if (personViewModel == null)
				return;

			if (!Persons.Contains(personViewModel))
				return;

			_unitOfWork.Persons.Remove(personViewModel.Model);
			await _unitOfWork.CompleteAsync();

			Persons.Remove(personViewModel);
		}

		private PersonViewModel CreateVm(Person person, int index)
		{
			var col = index % 5;
			var row = index / 5;

			return new PersonViewModel(person)
			{
				CanvasLeft = col * (NodeWidth + NodeMargin),
				CanvasTop = row * (NodeHeight + NodeMargin)
			};
		}
	}
}
