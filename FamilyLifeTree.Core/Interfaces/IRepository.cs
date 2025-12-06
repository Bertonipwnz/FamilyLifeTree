namespace FamilyLifeTree.Core.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

	/// <summary>
	/// Базовый интерфейс репозитория для работы с сущностями.
	/// </summary>
	public interface IRepository<T> where T : class
	{
		/// <summary>
		/// Получить сущность по идентификатору.
		/// </summary>
		Task<T?> GetByIdAsync(int id);

		/// <summary>
		/// Получить все сущности.
		/// </summary>
		Task<IEnumerable<T>> GetAllAsync();

		/// <summary>
		/// Найти сущности по условию.
		/// </summary>
		Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

		/// <summary>
		/// Получить первую сущность, удовлетворяющую условию, или null.
		/// </summary>
		Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);

		/// <summary>
		/// Добавить новую сущность.
		/// </summary>
		Task AddAsync(T entity);

		/// <summary>
		/// Добавить коллекцию сущностей.
		/// </summary>
		Task AddRangeAsync(IEnumerable<T> entities);

		/// <summary>
		/// Обновить существующую сущность.
		/// </summary>
		void Update(T entity);

		/// <summary>
		/// Удалить сущность.
		/// </summary>
		void Remove(T entity);

		/// <summary>
		/// Удалить коллекцию сущностей.
		/// </summary>
		void RemoveRange(IEnumerable<T> entities);

		/// <summary>
		/// Проверить, существует ли хотя бы одна сущность, удовлетворяющая условию.
		/// </summary>
		Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

		/// <summary>
		/// Получить количество всех сущностей.
		/// </summary>
		Task<int> CountAsync();

		/// <summary>
		/// Получить количество сущностей, удовлетворяющих условию.
		/// </summary>
		Task<int> CountAsync(Expression<Func<T, bool>> predicate);

		/// <summary>
		/// Сохранить изменения в базе данных.
		/// </summary>
		Task<int> SaveChangesAsync();
	}
}
