namespace FamilyLifeTree.UWP.Views.Pages
{
    using FamilyLifeTree.ViewModels.Pages;
    using Windows.UI.Xaml.Navigation;

    /// <summary>
    /// Основная страница приложения.
    /// </summary>
    public sealed partial class MainPage : BasePage
	{
		/// <summary>
		/// Инициалаизирует <see cref="MainPage"/>
		/// </summary>
		public MainPage()
		{
			InitializeComponent();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			App.CurrentApp?.SetNavigationFrame(_appFrame);
			DataContext = App.CurrentApp?.GetScopedService<MainPageViewModel>();

			base.OnNavigatedTo(e);
		}
	}
}
