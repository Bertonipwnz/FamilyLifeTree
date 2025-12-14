namespace FamilyLifeTree.ViewModels.Pages
{
	using FamilyLifeTree.Core.Interfaces;
	using System.Linq;
    using System.Threading.Tasks;
    using Utils.Interfaces;
	using Utils.Mvvm.ViewModels;

	/// <summary>
	/// Модель представления основной страницы.
	/// </summary>
	public class MainPageViewModel : BasePageViewModel
	{
		#region Private Fields

		/// <summary>
		/// Сервис навигации.
		/// </summary>
		private readonly INavigationService _navigationService;

		/// <summary>
		/// Unit of Work для работы с БД.
		/// </summary>
		private readonly IUnitOfWork _unitOfWork;

		/// <summary>
		/// Лоадер отображен.
		/// </summary>
		private bool _isLoaderVisibility = true;

		#endregion

		#region Public Properties

		/// <summary>
		/// <see cref="_isLoaderVisibility"/>
		/// </summary>
		public bool IsLoaderVisibility
		{
			get => _isLoaderVisibility;
			set => SetProperty(ref _isLoaderVisibility, value);
		}

		#endregion

		#region Public Constructors

		/// <summary>
		/// Создает экземпляр <see cref="MainPageViewModel"/>
		/// </summary>
		public MainPageViewModel(INavigationService navigationService, IUnitOfWork unitOfWork)
		{
			_navigationService = navigationService;
			_unitOfWork = unitOfWork;
		}

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public override async void OnNavigatedTo(object param = null)
		{
			var persons = await _unitOfWork.Persons.GetAllAsync();
			
			if (persons != null && persons.ToList().Count > 0)
			{
				_navigationService.NavigateTo<TreePageViewModel>();
			}
			else
			{
				_navigationService.NavigateTo<StartPageViewModel>();
			}

			base.OnNavigatedTo(param);

			//Для отображения интерфейса до скрытия лоадера.
			await Task.Delay(1000);

			IsLoaderVisibility = false;
		}

		#endregion
	}
}
