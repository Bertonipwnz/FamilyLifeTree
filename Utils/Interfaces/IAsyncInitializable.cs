namespace Utils.Interfaces
{
	using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс для объектов, поддерживающих асинхронную инициализацию.
    /// </summary>
    public interface IAsyncInitializable
	{
		/// <summary>
		/// Инициализирован ли сервис>
		/// </summary>
		public bool IsInited { get; set; }

		/// <summary>
		/// Событие инициализации.
		/// </summary>
		public EventHandler onInitialized { get; set; }

		/// <summary>
		/// Инициализирует объект.
		/// </summary>
		Task InitializeAsync();
	}
}
