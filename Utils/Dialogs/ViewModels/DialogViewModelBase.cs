namespace Utils.Dialogs.ViewModels
{
	using CommunityToolkit.Mvvm.Input;
	using System.Threading.Tasks;
	using Utils.Dialogs.Enums;

	/// <summary>
	/// Базовая модель представления диалога.
	/// </summary>
	public abstract class DialogViewModelBase
	{
		/// <summary>
		/// Источник завершения задачи.
		/// </summary>
		private TaskCompletionSource<DialogResult> _tcs;

		/// <summary>
		/// Ожидает завершения задачи диалога.
		/// </summary>
		public Task<DialogResult> WaitAsync() => _tcs.Task;

		/// <summary>
		/// Команда основного действия диалога.
		/// </summary>
		public IAsyncRelayCommand PrimaryCommand { get; }

		/// <summary>
		/// Комнда отмены диалога.
		/// </summary>
		public IAsyncRelayCommand CancelCommand { get; }

		/// <summary>
		/// Команда закрытия диалога.
		/// </summary>
		public IAsyncRelayCommand CloseCommand { get; }

		/// <summary>
		/// Контент диалога.
		/// </summary>
		public object Content { get; set; }

		/// <summary>
		/// Создает экземпляр <see cref="DialogViewModelBase"/>
		/// </summary>
		public DialogViewModelBase()
		{
			_tcs = new TaskCompletionSource<DialogResult>();
			PrimaryCommand = new AsyncRelayCommand(OnPrimaryCommandExecutedAsync);
			CancelCommand = new AsyncRelayCommand(OnCancelCommandExecutedAsync);
			CloseCommand = new AsyncRelayCommand(OnCloseCommandExecutedAsync);
		}

		/// <summary>
		/// <see cref="PrimaryCommand"/>
		/// </summary>
		protected virtual async Task OnPrimaryCommandExecutedAsync()
		{
			SetResult(DialogResult.Primary);
		}

		/// <summary>
		/// <see cref="CancelCommand"/>
		/// </summary>
		protected virtual async Task OnCancelCommandExecutedAsync()
		{
			SetResult(DialogResult.Cancel);
		}

		/// <summary>
		/// <see cref="CloseCommand"/>
		/// </summary>
		protected virtual async Task OnCloseCommandExecutedAsync()
		{
			SetResult(DialogResult.Close);
		}

		/// <summary>
		/// Устанавливает результат диалога.
		/// </summary>
		/// <param name="result">Результат.</param>
		private void SetResult(DialogResult result)
		{
			_tcs?.TrySetResult(result);
		}
	}
}
