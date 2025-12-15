namespace FamilyLifeTree.ViewModels.Pages
{
    using CommunityToolkit.Mvvm.Input;
    using FamilyLifeTree.Core.Enums;
    using FamilyLifeTree.Core.Interfaces;
    using FamilyLifeTree.Core.Models;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Utils.Interfaces;
    using Utils.Mvvm.ViewModels;

#nullable enable

    /// <summary>
    /// Модель представления стартовой страницы.
    /// </summary>
    public class StartPageViewModel : BasePageViewModel
	{
		#region Private Fields

		/// <summary>
		/// Unit of Work для работы с БД.
		/// </summary>
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Сервис навигации.
		/// </summary>
		private readonly INavigationService _navigationService;

		/// <summary>
		/// Имя.
		/// </summary>
		private string _firstName = string.Empty;

		/// <summary>
		/// Фамилия.
		/// </summary>
		private string _lastName = string.Empty;

		/// <summary>
		/// Отчество.
		/// </summary>
		private string? _middleName;

		/// <summary>
		/// Дата рождения.
		/// </summary>
		private DateTimeOffset? _birthDate;

		/// <summary>
		/// Дата смерти.
		/// </summary>
		private DateTimeOffset? _deathDate;

		/// <summary>
		/// Гендер.
		/// </summary>
		private Gender _gender = Gender.Unknown;

		/// <summary>
		/// Биография.
		/// </summary>
		private string? _biography;

		#endregion

		#region Public Properties

		/// <summary>
		/// <see cref="_firstName"/>
		/// </summary>
		public string FirstName
		{
			get => _firstName;
			set => SetProperty(ref _firstName, value);
		}

		/// <summary>
		/// Команда создания персоны и начала построения дерева.
		/// </summary>
		public IAsyncRelayCommand CreatePersonCommand { get; }

		/// <summary>
		/// Доступные значения гендера для ComboBox.
		/// </summary>
		public Gender[] Genders => Enum.GetValues(typeof(Gender)).Cast<Gender>().ToArray();

		#endregion

		#region Public Constructors

		/// <summary>
		/// Создает экземпляр <see cref="StartPageViewModel"/>
		/// </summary>
		/// <param name="unitOfWork">Unit of Work для работы с БД.</param>
		/// <param name="navigationService">Сервис навигации.</param>
		public StartPageViewModel(IUnitOfWork unitOfWork, INavigationService navigationService)
		{
			_unitOfWork = unitOfWork;
			_navigationService = navigationService;
			CreatePersonCommand = new AsyncRelayCommand(CreatePersonAsync, CanCreatePerson);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Создает персону и переходит к дереву.
		/// </summary>
		private async Task CreatePersonAsync()
		{
			if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
				return;

			var person = new Person
			{
				FirstName = FirstName.Trim(),
				LastName = LastName.Trim(),
				MiddleName = string.IsNullOrWhiteSpace(MiddleName) ? null : MiddleName.Trim(),
				BirthDate = BirthDate?.DateTime,
				DeathDate = DeathDate?.DateTime,
				Gender = Gender,
				Biography = string.IsNullOrWhiteSpace(Biography) ? null : Biography.Trim()
			};

			await _unitOfWork.Persons.AddAsync(person);
			await _unitOfWork.CompleteAsync();

			_navigationService.NavigateTo<TreePageViewModel>();
		}

		/// <summary>
		/// Проверяет, можно ли создать персону.
		/// </summary>
		private bool CanCreatePerson()
		{
			return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName);
		}

		#endregion

		#region Partial Methods

		/// <summary>
		/// Обработчик изменения свойства FirstName.
		/// </summary>
		partial void OnFirstNameChanged(string value)
		{
			CreatePersonCommand.NotifyCanExecuteChanged();
		}

		/// <summary>
		/// Обработчик изменения свойства LastName.
		/// </summary>
		partial void OnLastNameChanged(string value)
		{
			CreatePersonCommand.NotifyCanExecuteChanged();
		}

		#endregion
	}
}
