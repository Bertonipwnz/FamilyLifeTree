namespace Utils.Dialogs.ViewModels
{
	using CommunityToolkit.Mvvm.Input;
	using System.Threading.Tasks;
	using Utils.Dialogs.Enums;

	public abstract class DialogViewModelBase
	{
		private TaskCompletionSource<DialogResult> _tcs;

		public Task<DialogResult> WaitAsync() => _tcs.Task;

		public IAsyncRelayCommand PrimaryCommand { get; }

		public IAsyncRelayCommand CancelCommand { get; }

		public IAsyncRelayCommand CloseCommand { get; }

		public object Content { get; set; }

		public DialogViewModelBase()
		{
			_tcs = new TaskCompletionSource<DialogResult>();
			PrimaryCommand = new AsyncRelayCommand(OnPrimaryCommandExecutedAsync);
			CancelCommand = new AsyncRelayCommand(OnCancelCommandExecutedAsync);
			CloseCommand = new AsyncRelayCommand(OnCloseCommandExecutedAsync);
		}

		public async Task ShowDialogAsync() => await WaitAsync();

		protected virtual async Task OnPrimaryCommandExecutedAsync()
		{
			SetResult(DialogResult.Primary);
		}

		protected virtual async Task OnCancelCommandExecutedAsync()
		{
			SetResult(DialogResult.Cancel);
		}

		protected virtual async Task OnCloseCommandExecutedAsync()
		{
			SetResult(DialogResult.Close);
		}

		private void SetResult(DialogResult result)
		{
			_tcs?.TrySetResult(result);
		}
	}
}
