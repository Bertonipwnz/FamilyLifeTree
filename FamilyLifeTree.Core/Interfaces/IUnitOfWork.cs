namespace FamilyLifeTree.Core.Interfaces
{
	using System;
	using System.Threading.Tasks;

	/// <summary>
	/// Интерфейс Unit of Work для управления репозиториями и транзакциями.
	/// </summary>
	public interface IUnitOfWork : IDisposable
	{
		/// <summary>
		/// Репозиторий для работы с персонами.
		/// </summary>
		IPersonRepository Persons { get; }

		/// <summary>
		/// Репозиторий для работы со связями.
		/// </summary>
		IRelationshipRepository Relationships { get; }

		/// <summary>
		/// Сохранить все изменения в базе данных.
		/// </summary>
		/// <returns>Количество изменённых записей.</returns>
		Task<int> CompleteAsync();

		/// <summary>
		/// Начать новую транзакцию.
		/// </summary>
		Task BeginTransactionAsync();

		/// <summary>
		/// Зафиксировать текущую транзакцию.
		/// </summary>
		Task CommitTransactionAsync();

		/// <summary>
		/// Откатить текущую транзакцию.
		/// </summary>
		Task RollbackTransactionAsync();
	}
}
