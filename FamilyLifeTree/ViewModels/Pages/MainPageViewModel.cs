namespace FamilyLifeTree.ViewModels.Pages
{
    using Utils.Interfaces;
    using Utils.Mvvm.ViewModels;

    /// <summary>
    /// Модель представления основной страницы.
    /// </summary>
	public class MainPageViewModel : BasePageViewModel
	{
		/// <summary>
		/// Сервис навигации.
		/// </summary>
		private readonly INavigationService _navigationService;

		/// <summary>
		/// Создает экземпляр <see cref="MainPageViewModel"/>
		/// </summary>
		/// <param name="navigationService">Сервис навигации.</param>
		public MainPageViewModel(INavigationService navigationService)
		{
			_navigationService = navigationService;
		}

		/// <inheritdoc/>
		public override void OnNavigatedTo(object param = null)
		{
			_navigationService.NavigateTo<TreePageViewModel>();
		}
	}
}
