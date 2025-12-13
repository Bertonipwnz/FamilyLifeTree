namespace FamilyLifeTree.UWP.Views.Pages
{
	using FamilyLifeTree.ViewModels.Pages;
	using Windows.UI.Xaml.Navigation;

	/// <summary>
	/// Страница с древом.
	/// </summary>
	public sealed partial class TreePage : BasePage
	{
		#region Public Constructors

		/// <summary>
		/// Инициалазирует <see cref="TreePage"/>
		/// </summary>
		public TreePage()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Protected Methods

		/// <inheritdoc/>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			DataContext = App.CurrentApp?.GetRequiredService<TreePageViewModel>();
			base.OnNavigatedTo(e);
		}

		#endregion
	}
}
