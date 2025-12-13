namespace Utils.Mvvm.ViewModels
{
	using CommunityToolkit.Mvvm.ComponentModel;
	using System;

	/// <summary>
	/// Базовая модель представления страницы.
	/// </summary>
	public class BasePageViewModel : ObservableObject, IDisposable
	{
		#region Private Fields

		/// <summary>
		/// Уничтожены ли ресурсы?
		/// </summary>
		private bool _isDisposed = false;

		#endregion

		#region Public Methods

		/// <summary>
		/// Обрабатывает навигацию на страницу.
		/// <paramref name="param">Передаваемый параметр при навигации.</paramref>
		/// </summary>
		public virtual void OnNavigatedTo(object param = null)
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
		public virtual void Dispose()
		{
			if (_isDisposed)
				return;

			_isDisposed = true;
		}

		#endregion
	}
}
