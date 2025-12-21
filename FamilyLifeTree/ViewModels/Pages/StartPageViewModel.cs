namespace FamilyLifeTree.ViewModels.Pages
{
	using CommunityToolkit.Mvvm.Input;
	using FamilyLifeTree.Core.Enums;
	using FamilyLifeTree.Core.Interfaces;
	using FamilyLifeTree.Core.Models;
	using FamilyLifeTree.ViewModels.Entities;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using Utils.Extensions;
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
		/// Сервис локализаций
		/// </summary>
		private readonly ILocalizationService _localizationService;

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
		/// Гендеры.
		/// </summary>
		public List<GenderViewModel> Genders { get; private set; } = new List<GenderViewModel>();

		/// <summary>
		/// <see cref="_firstName"/>
		/// </summary>
		public string FirstName
		{
			get => _firstName;
			set
			{
				if (SetProperty(ref _firstName, value))
					CreatePersonCommand?.NotifyCanExecuteChanged();
			}
		}

		/// <summary>
		/// <see cref="_lastName"/>
		/// </summary>
		public string LastName
		{
			get => _lastName;
			set
			{
				if (SetProperty(ref _lastName, value))
					CreatePersonCommand?.NotifyCanExecuteChanged();
			}
		}

		/// <summary>
		/// <see cref="_middleName"/>
		/// </summary>
		public string? MiddleName
		{
			get => _middleName;
			set => SetProperty(ref _middleName, value);
		}

		/// <summary>
		/// <see cref="_birthDate"/>
		/// </summary>
		public DateTimeOffset? BirthDate
		{
			get => _birthDate;
			set
			{
				if (SetProperty(ref _birthDate, value))
					CreatePersonCommand?.NotifyCanExecuteChanged();
			}
		}

		/// <summary>
		/// <see cref="_deathDate"/>
		/// </summary>
		public DateTimeOffset? DeathDate
		{
			get => _deathDate;
			set
			{
				if (SetProperty(ref _deathDate, value))
					CreatePersonCommand?.NotifyCanExecuteChanged();
			}
		}

		/// <summary>
		/// <see cref="_gender"/>
		/// </summary>
		public Gender Gender
		{
			get => _gender;
			set => SetProperty(ref _gender, value);
		}

		/// <summary>
		/// <see cref="_biography"/>
		/// </summary>
		public string? Biography
		{
			get => _biography;
			set => SetProperty(ref _biography, value);
		}

		/// <summary>
		/// Команда создания персоны и начала построения дерева.
		/// </summary>
		public IAsyncRelayCommand CreatePersonCommand { get; }

		/// <summary>
		/// Валидно ли имя?
		/// </summary>
		public bool IsValidFirstName => FirstName.IsLettersOnlyName();

		/// <summary>
		/// Валидна ли фамилия?
		/// </summary>
		public bool IsValidLastName => LastName.IsLettersOnlyName();

		/// <summary>
		/// Валидна ли дата смерти?
		/// </summary>
		public bool IsValidDeathDate => DeathDate.IsValidDeathDate(BirthDate);

		/// <summary>
		/// Валидна ли дата рождения?
		/// </summary>
		public bool IsValidBirthDate => BirthDate.IsValidDate();

		/// <summary>
		/// Поле ошибок валидации полей.
		/// </summary>
		public string ErrorFields
		{
			get
			{
				var errors = new List<string>();

				if (!IsValidFirstName) 
					errors.Add(_localizationService.GetLocalizationString("NameInputError"));

				if (!IsValidLastName) 
					errors.Add(_localizationService.GetLocalizationString("SecondNameInputError"));

				if (!IsValidBirthDate) 
					errors.Add(_localizationService.GetLocalizationString("DateBirthdayError"));

				if (!IsValidDeathDate) 
					errors.Add(_localizationService.GetLocalizationString("DateDeathError"));

				return string.Join(Environment.NewLine, errors);
			}
		}

		#endregion

		#region Public Constructors

		/// <summary>
		/// Создает экземпляр <see cref="StartPageViewModel"/>
		/// </summary>
		/// <param name="unitOfWork">Unit of Work для работы с БД.</param>
		/// <param name="navigationService">Сервис навигации.</param>
		/// <param name="localizationService">Сервис локализаций.</param>
		public StartPageViewModel(IUnitOfWork unitOfWork, INavigationService navigationService, ILocalizationService localizationService, IEntityService<GenderModel, GenderViewModel> genderSerivce)
		{
			_unitOfWork = unitOfWork;
			_navigationService = navigationService;
			_localizationService = localizationService;
			Genders = genderSerivce.ViewModels.ToList();
			CreatePersonCommand = new AsyncRelayCommand(OnExecutedCommandCreatePersonAsync, CanExecuteCommandCreatePersonAsync);
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Создает персону и переходит к дереву.
		/// </summary>
		private async Task OnExecutedCommandCreatePersonAsync()
		{
			if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
				return;

			var person = new Person
			{
				FirstName = FirstName.Trim(),
				LastName = LastName.Trim(),
				MiddleName = string.IsNullOrWhiteSpace(MiddleName) ? null : MiddleName?.Trim(),
				BirthDate = BirthDate?.DateTime,
				DeathDate = DeathDate?.DateTime,
				Gender = Gender,
				Biography = string.IsNullOrWhiteSpace(Biography) ? null : Biography?.Trim()
			};

			await _unitOfWork.Persons.AddAsync(person);
			await _unitOfWork.CompleteAsync();

			_navigationService.NavigateTo<TreePageViewModel>();
		}

		/// <summary>
		/// Проверяет, можно ли создать персону.
		/// </summary>
		private bool CanExecuteCommandCreatePersonAsync()
		{
			OnPropertyChanged(nameof(IsValidFirstName));
			OnPropertyChanged(nameof(IsValidLastName));
			OnPropertyChanged(nameof(IsValidDeathDate));
			OnPropertyChanged(nameof(IsValidBirthDate));
			OnPropertyChanged(nameof(ErrorFields));

			return IsValidFirstName && IsValidLastName && IsValidDeathDate && IsValidBirthDate;
		}

		#endregion
	}
}
