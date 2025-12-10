namespace FamilyLifeTree.UWP.Services
{
	using FamilyLifeTree.UWP.Views.Pages;
	using FamilyLifeTree.ViewModels.Pages;
	using Serilog;
	using System;
	using System.Collections.Generic;
	using Utils.Interfaces;
	using Utils.Logger;
	using Windows.UI.Xaml.Controls;

	/// <summary>
	/// Сервис навигации для UWP. Платформозависимая реализация.
	/// </summary>
	public sealed class UWPNavigationService : INavigationService
	{
		/// <summary>
		/// Логгер текущего класса.
		/// </summary>
		private readonly ILogger _logger = LogService.GetCurrentLogger();

		/// <summary>
		/// Основной фрейм приложения, выполняющий навигацию.
		/// </summary>
		private Frame? _frame;

		/// <summary>
		/// Словарь сопоставления ViewModel → Page.
		/// </summary>
		private readonly Dictionary<Type, Type> _pageMap;

		/// <summary>
		/// Признак того, что сервис уже освобождён.
		/// </summary>
		private bool _isDisposed = false;

		/// <summary>
		/// Создаёт экземпляр сервиса навигации.
		/// </summary>
		public UWPNavigationService()
		{
			_pageMap = new Dictionary<Type, Type>
			{
				{ typeof(TreePageViewModel), typeof(TreePage) },
			};

			_logger?.Debug("NavigationService создан и готов к работе");
		}

		/// <summary>
		/// Инициализирует сервис навигации указанным Frame.
		/// Вызывается один раз из App.SetNavigationFrame().
		/// </summary>
		internal void Initialize(Frame frame)
		{
			if (_frame != null)
				throw new InvalidOperationException("NavigationService уже инициализирован.");

			if (frame == null) 
				throw new ArgumentNullException(nameof(frame));

			_frame = frame;

			_logger?.Debug("NavigationService инициализирован с пользовательским Frame");
		}

		/// <summary>
		/// Выполняет переход к странице, связанной с указанной моделью представления.
		/// </summary>
		/// <typeparam name="TViewModel">Тип модели представления, к которой нужно перейти.</typeparam>
		/// <param name="parameter">Необязательный параметр, передаваемый на страницу.</param>
		/// <exception cref="KeyNotFoundException">Выбрасывается, если для TViewModel не зарегистрирована страница.</exception>
		public void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : class
		{
			if (_frame == null)
				return;

			if (!_pageMap.TryGetValue(typeof(TViewModel), out var pageType))
			{
				_logger?.Error("Не найдено сопоставление для ViewModel {ViewModelType}", typeof(TViewModel));
				throw new KeyNotFoundException($"Нет зарегистрированной страницы для {typeof(TViewModel)}");
			}

			_logger?.Debug("Навигация → {PageType} (VM: {ViewModelType})", pageType.Name, typeof(TViewModel).Name);
			_frame.Navigate(pageType, parameter);
		}

		/// <summary>
		/// Возвращается на предыдущую страницу, если это возможно.
		/// </summary>
		public void GoBack()
		{
			if (_frame == null)
				return;

			if (_frame.CanGoBack)
			{
				_logger?.Debug("Выполняется возврат назад");
			}
			else
			{
				_logger?.Warning("Возврат назад невозможен — история пуста");
			}

			if (_frame.CanGoBack)
				_frame.GoBack();
		}

		/// <summary>
		/// Освобождает ресурсы сервиса.
		/// </summary>
		public void Dispose()
		{
			if (_isDisposed)
				return;

			_isDisposed = true;
			_logger?.Debug("NavigationService disposed");
		}
	}
}