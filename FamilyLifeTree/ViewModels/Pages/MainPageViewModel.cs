namespace FamilyLifeTree.ViewModels.Pages
{
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

		#endregion

		#region Public Constructors

		/// <summary>
		/// Создает экземпляр <see cref="MainPageViewModel"/>
		/// </summary>
		/// <param name="navigationService">Сервис навигации.</param>
		public MainPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public override void OnNavigatedTo(object param = null)
		{
			_navigationService.NavigateTo<TreePageViewModel>();
			base.OnNavigatedTo(param);
		}

		#endregion
	}
}
