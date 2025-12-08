namespace Utils.Interfaces
{
    using System;

    /// <summary>
    /// Интерфейс представляющий сервис навигации.
    /// </summary>
    /// <typeparam name="H">Хост.</typeparam>
    /// <typeparam name="P">Страница.</typeparam>
    public interface INavigationService<H, P> : IDisposable
	{
		/// <summary>
		/// Устанавливает хост.
		/// </summary>
		/// <param name="host">Экземпляр хоста.</param>
		public void SetHost(H host);

		/// <summary>
		/// Обрабатывает навигацию на страницу.
		/// </summary>
		/// <param name="page">Страница.</param>
		/// <param name="param">Передаваемый параметр.</param>
		public void NavigateToPage(P page, object param);
	}
}
