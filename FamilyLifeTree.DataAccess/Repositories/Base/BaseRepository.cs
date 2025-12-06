namespace FamilyLifeTree.DataAccess.Repositories.Base
{
	using FamilyLifeTree.Core.Interfaces;
	using FamilyLifeTree.DataAccess.DbContext;
	using Microsoft.EntityFrameworkCore;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;

#nullable enable

	/// <summary>
	/// Базовый репозиторий для работы с сущностями через Entity Framework Core.
	/// </summary>
	/// <typeparam name="TEntity">Тип сущности Entity Framework</typeparam>
	public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		#region Protected Fields

		/// <summary>
		/// Экземпляр контекста БД.
		/// </summary>
		protected readonly FamilyTreeDbContext _context;

		/// <summary>
		/// Сущности.
		/// </summary>
		protected readonly DbSet<TEntity> _dbSet;

		#endregion Protected Fields

		#region Protected Methods

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="BaseRepository{TEntity}"/>.
		/// </summary>
		/// <param name="context">Контекст базы данных</param>
		protected BaseRepository(FamilyTreeDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_dbSet = context.Set<TEntity>();
		}

		/// <inheritdoc/>
		public virtual async Task<TEntity?> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}

		/// <inheritdoc/>
		public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await _dbSet.ToListAsync();
		}

		/// <inheritdoc/>
		public virtual async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await _dbSet.Where(predicate).ToListAsync();
		}

		/// <inheritdoc/>
		public virtual async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await _dbSet.FirstOrDefaultAsync(predicate);
		}

		/// <inheritdoc/>
		public virtual async Task AddAsync(TEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			await _dbSet.AddAsync(entity);
		}

		/// <inheritdoc/>
		public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
		{
			if (entities == null)
				throw new ArgumentNullException(nameof(entities));

			await _dbSet.AddRangeAsync(entities);
		}

		/// <inheritdoc/>
		public virtual void Update(TEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			_dbSet.Update(entity);
		}

		/// <inheritdoc/>
		public virtual void Remove(TEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			_dbSet.Remove(entity);
		}

		/// <inheritdoc/>
		public virtual void RemoveRange(IEnumerable<TEntity> entities)
		{
			if (entities == null)
				throw new ArgumentNullException(nameof(entities));

			_dbSet.RemoveRange(entities);
		}

		/// <inheritdoc/>
		public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await _dbSet.AnyAsync(predicate);
		}

		/// <inheritdoc/>
		public virtual async Task<int> CountAsync()
		{
			return await _dbSet.CountAsync();
		}

		/// <inheritdoc/>
		public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return await _dbSet.CountAsync(predicate);
		}

		/// <inheritdoc/>
		public virtual async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		#endregion Protected Methods
	}
}
