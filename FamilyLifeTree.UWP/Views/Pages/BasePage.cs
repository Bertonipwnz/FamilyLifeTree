namespace FamilyLifeTree.UWP.Views.Pages
{
	using Utils.Mvvm.ViewModels;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Navigation;

	/// <summary>
	/// Базовая страница.
	/// </summary>
	public partial class BasePage : Page
	{
		#region Protected Methods

		/// <inheritdoc/>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if(DataContext is BasePageViewModel viewModel)
			{
				viewModel.OnNavigatedTo();
			}

			base.OnNavigatedTo(e);
		}

		/// <inheritdoc/>
		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			if (DataContext is BasePageViewModel viewModel)
			{
				viewModel.OnNavigatedFrom();
			}

			base.OnNavigatedFrom(e);
		}

		#endregion
	}
}
