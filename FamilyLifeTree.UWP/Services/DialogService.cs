using FamilyLifeTree.UWP.Controls;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FamilyLifeTree.UWP.Services
{
	public class DialogService : IDialogService<UserControl>
	{
		private readonly IServiceProvider _serviceProvider;
		private IDialogHost<UserControl> _host;

		public DialogService(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public void SetHost(IDialogHost<UserControl> host)
		{
			_host = host;
		}

		public async Task ShowAsync(DialogViewModelBase viewModel)
		{
			if (_host == null)
				throw new InvalidOperationException("Dialog host is not set.");

			// Получаем View через DI
			var viewType = ResolveViewType(viewModel.GetType());
			var view = (UserControl)_serviceProvider.GetService(viewType);
			view.DataContext = viewModel;

			viewModel.AttachService(this);

			_host.ShowDialog(view);
			await Task.CompletedTask;
		}

		public void Close(DialogViewModelBase viewModel)
		{
			var viewType = ResolveViewType(viewModel.GetType());
			var view = FindOpenedView(viewType);

			if (view != null)
				_host.CloseDialog(view);
		}

		private Type ResolveViewType(Type viewModelType)
		{
			var name = viewModelType.FullName.Replace("ViewModel", "View");
			return Type.GetType(name);
		}

		private UserControl FindOpenedView(Type viewType)
		{
			// Реализация зависит от хоста
			return _host switch
			{
				DialogHost host => host.FindView(viewType),
				_ => null
			};
		}
	}

}
