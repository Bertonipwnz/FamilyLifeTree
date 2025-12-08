namespace Utils.Mvvm.ViewModels
{
	using CommunityToolkit.Mvvm.ComponentModel;
	using System;

	/// <summary>
	/// Базовая модель представления страницы.
	/// </summary>
	public class BasePageViewModel : ObservableObject, IDisposable
	{
		/// <summary>
		/// Уничтожены ли ресурсы?
		/// </summary>
		private bool _isDisposed = false;

		/// <summary>
		/// Обрабатывает навигацию на страницу.
		/// </summary>
		public virtual void OnNavigatedTo()
		{

		}

		/// <summary>
		/// Обрабатывает навигацию со страницы.
		/// </summary>
		public virtual void OnNavigatedFrom()
		{
			Dispose();
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			if (_isDisposed)
				return;

			_isDisposed = true;
		}
	}
}
