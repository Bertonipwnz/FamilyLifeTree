namespace Utils.Interfaces
{
	using System;

#nullable enable

	/// <summary>
	/// Интерфейс сервиса навигации (платформо-независимый).
	/// </summary>
	public interface INavigationService : IDisposable
	{
		/// <summary>
		/// Переходит к странице, связанной с указанной моделью представления.
		/// </summary>
		/// <typeparam name="TViewModel">Тип модели представления.</typeparam>
		/// <param name="parameter">Параметр навигации.</param>
		void NavigateTo<TViewModel>(object? parameter = null) where TViewModel : class;

		/// <summary>
		/// Возвращается на предыдущую страницу (если возможно).
		/// </summary>
		void GoBack();
	}
}
