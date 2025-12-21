namespace Utils.Interfaces
{
	using System.Collections.Generic;

	/// <summary>
	/// Интерфейс сервиса сущности.
	/// </summary>
	/// <typeparam name="M">Модель.</typeparam>
	/// <typeparam name="VM">Модель представления.</typeparam>
	public interface IEntityService<M,VM> : IAsyncInitializable
	{
		/// <summary>
		/// Создает модель.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		VM CreateVM(M model);

		/// <summary>
		/// Список моделей представления.
		/// </summary>
		public IEnumerable<VM> ViewModels { get; }
	}
}
