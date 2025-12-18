namespace FamilyLifeTree.UWP.Services
{
	using Serilog;
	using System;
	using System.Threading.Tasks;
	using Utils.Dialogs.Enums;
	using Utils.Dialogs.Services;
	using Utils.Dialogs.ViewModels;
	using Utils.Logger;
	using Windows.UI;
	using Windows.UI.Xaml.Controls;
	using Windows.UI.Xaml.Media;

#nullable enable

	//TODO: Логгирование.
	/// <summary>
	/// Сервис диалогов.
	/// </summary>
	public class DialogService : IDialogService
	{
		#region Private Fields

		/// <summary>
		/// Хост.
		/// </summary>
		private ContentPresenter? _host;

		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger? _logger = LogService.GetCurrentLogger();

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public void SetHost(object host)
		{
			if (host is ContentPresenter contentPresenter)
			{
				_host = contentPresenter;
				_logger?.Debug("Host was setted");
				return;
			}

			throw new InvalidOperationException("host is not ContentPresenter.");
		}

		/// <inheritdoc/>
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

		#endregion
	}
}
