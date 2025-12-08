namespace Utils.Interfaces
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Интерфейс для объектов, поддерживающих асинхронную инициализацию.
	/// </summary>
	public interface IAsyncInitializable
	{
		/// <summary>
		/// Получает значение, указывающее, инициализирован ли объект.
		/// </summary>
		bool IsInitialized { get; }

		/// <summary>
		/// Событие, возникающее после успешной инициализации объекта.
		/// </summary>
		event EventHandler Initialized;

		/// <summary>
		/// Выполняет асинхронную инициализацию объекта с поддержкой отмены.
		/// </summary>
		/// <param name="cancellationToken">Токен отмены для прерывания операции.</param>
		/// <returns>Задача, представляющая операцию инициализации.</returns>
		Task InitializeAsync(CancellationToken cancellationToken = default);
	}
}
