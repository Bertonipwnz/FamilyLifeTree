namespace Utils.Dialogs.Services
{
    using System;
    using System.Threading.Tasks;
    using Utils.Dialogs.Enums;
    using Utils.Dialogs.Models.Interfaces;
    using Utils.Dialogs.ViewModels;

    public class DialogService
	{
		private IDialogHostUI _host;

		public void SetHost(IDialogHostUI host)
			=> _host = host;

		public async Task<DialogResult> ShowAsync(DialogViewModelBase vm)
		{
			if (_host == null)
				throw new InvalidOperationException("Dialog host not set");

			vm.Attach();

			_host.Show(vm);

			var result = await vm.WaitAsync();

			_host.Close(vm);

			return result;
		}
	}

}
