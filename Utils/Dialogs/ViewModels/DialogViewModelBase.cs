namespace Utils.Dialogs.ViewModels
{
	using System;
	using System.Threading.Tasks;
	using Utils.Dialogs.Enums;

	public abstract class DialogViewModelBase
	{
		private TaskCompletionSource<DialogResult> _tcs;

		internal void Attach()
		{
			_tcs = new TaskCompletionSource<DialogResult>();
		}

		public Task<DialogResult> WaitAsync() => _tcs.Task;

		protected void SetResult(DialogResult result)
		{
			_tcs.TrySetResult(result);
			Closed?.Invoke(this);
		}

		public void Close() => SetResult(DialogResult.Close);

		internal event Action<DialogViewModelBase> Closed;
	}
}
