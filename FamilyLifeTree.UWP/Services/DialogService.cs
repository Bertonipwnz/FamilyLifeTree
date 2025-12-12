using System;
using System.Threading.Tasks;
using Utils.Dialogs.Enums;
using Utils.Dialogs.Services;
using Utils.Dialogs.ViewModels;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace FamilyLifeTree.UWP.Services
{
	public class DialogService : IDialogService
	{
		private ContentPresenter _host;

		public void SetHost(object host)
		{
			if (host is ContentPresenter contentPresenter)
			{
				_host = contentPresenter;
				return;
			}

			throw new InvalidOperationException("host is not ContentPresenter.");
		}

		public async Task<DialogResult> ShowAsync(DialogViewModelBase vm)
		{
			if (_host == null)
				throw new InvalidOperationException("Dialog host not configured.");

			if (vm.Content is not UserControl control)
				throw new InvalidOperationException("Content is not UserControl.");

			if (_host.Content == null)
			{
				_host.Content = new Grid()
				{
					Background = new SolidColorBrush(Colors.Transparent)
				};
			}

			DialogResult result = DialogResult.None;

			if (_host.Content is Grid grid)
			{
				grid.Children.Add(control);

				result = await vm.WaitAsync();

				grid.Children.Remove(control);
			}

			return result;
		}
	}
}
