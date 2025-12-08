namespace FamilyLifeTree.UWP
{
	using FamilyLifeTree.ViewModels.Pages;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Navigation;

	/// <summary>
	/// Основная страница приложения.
	/// </summary>
	public sealed partial class MainPage : Page
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
			base.OnNavigatedTo(e);

			App.CurrentApp?.SetNavigationFrame(_appFrame);
			DataContext = App.CurrentApp?.GetRequiredService<MainPageViewModel>();
		}
	}
}
