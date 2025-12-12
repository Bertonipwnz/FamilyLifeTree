namespace FamilyLifeTree.UWP.Views.Pages
{
	using FamilyLifeTree.ViewModels.Pages;
	using Utils.Dialogs.Services;
	using Windows.UI.Xaml.Navigation;

	/// <summary>
	/// Основная страница приложения.
	/// </summary>
	public sealed partial class MainPage : BasePage
	{
		#region Public Constructors

		/// <summary>
		/// Инициалаизирует <see cref="MainPage"/>
		/// </summary>
		public MainPage()
		{
			InitializeComponent();
		}

		#endregion

		#region Protected Methods

		/// <inheritdoc/>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (App.CurrentApp is App app)
			{
				app?.SetNavigationFrame(_appFrame);
				DataContext = app?.GetScopedService<MainPageViewModel>();

				if (app?.GetRequiredService<IDialogService>() is IDialogService dialogService)

				{
					dialogService.SetHost(DialogPresenter);
				}
			}

			base.OnNavigatedTo(e);
		}

		#endregion
	}
}
