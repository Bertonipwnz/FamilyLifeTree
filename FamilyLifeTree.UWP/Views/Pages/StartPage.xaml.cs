namespace FamilyLifeTree.UWP.Views.Pages
{
	using FamilyLifeTree.ViewModels.Pages;
	using Windows.UI.Xaml.Navigation;

	/// <summary>
	/// Стартовая страница.
	/// </summary>
	public sealed partial class StartPage : BasePage
	{
		#region Public Constructors

		/// <summary>
		/// Инициализирует <see cref="StartPage"/>
		/// </summary>
		public StartPage()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Protected Methods

		/// <inheritdoc/>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			DataContext = App.CurrentApp?.GetRequiredService<StartPageViewModel>();
			base.OnNavigatedTo(e);
		}

		#endregion
	}
}
