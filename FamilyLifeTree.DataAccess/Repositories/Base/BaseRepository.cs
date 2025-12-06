namespace FamilyLifeTree.DataAccess.Repositories.Base
{
	using AutoMapper;
	using AutoMapper.Extensions.ExpressionMapping;
	using AutoMapper.QueryableExtensions;
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
	/// Поддерживает автоматическое преобразование Expression с помощью AutoMapper.
	/// </summary>
	/// <typeparam name="TEntity">Тип сущности Entity Framework</typeparam>
	/// <typeparam name="TModel">Тип модели бизнес-логики</typeparam>
	public abstract class BaseRepository<TEntity, TModel> : IRepository<TModel>
			where TEntity : class
			where TModel : class
	{
		#region Protected Fields

		/// <summary>
		/// Экземпляр контекста БД.
		/// </summary>
		protected readonly FamilyTreeDbContext _context;

		/// <summary>
		/// Маппер для преобразования между сущностями и моделями.
		/// </summary>
		protected readonly IMapper _mapper;

		/// <summary>
		/// Набор данных сущностей.
		/// </summary>
		protected readonly DbSet<TEntity> _dbSet;

		#endregion Protected Fields

		#region Constructor

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="BaseRepository{TEntity, TModel}"/>.
		/// </summary>
		/// <param name="context">Контекст базы данных</param>
		/// <param name="mapper">Маппер для преобразования сущностей</param>
		/// <exception cref="ArgumentNullException">Выбрасывается, если context или mapper равны null</exception>
		protected BaseRepository(FamilyTreeDbContext context, IMapper mapper)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
			_dbSet = context.Set<TEntity>();
		}

		#endregion Constructor

		#region IRepository<TModel> Implementation

		/// <inheritdoc/>
		public virtual async Task<TModel?> GetByIdAsync(int id)
		{
			var entity = await _dbSet.FindAsync(id);
			return entity == null ? null : _mapper.Map<TModel>(entity);
		}

		/// <inheritdoc/>
		public virtual async Task<IEnumerable<TModel>> GetAllAsync()
		{
			return await _dbSet
				.ProjectTo<TModel>(_mapper.ConfigurationProvider)
				.ToListAsync();
		}

		/// <inheritdoc/>
		public virtual async Task<IEnumerable<TModel>> FindAsync(Expression<Func<TModel, bool>> predicate)
		{
			// Преобразуем предикат для модели в предикат для сущности
			var entityPredicate = _mapper.MapExpression<Expression<Func<TEntity, bool>>>(predicate);

			return await _dbSet
				.Where(entityPredicate)
				.ProjectTo<TModel>(_mapper.ConfigurationProvider)
				.ToListAsync();
		}

		/// <inheritdoc/>
		public virtual async Task<TModel?> FirstOrDefaultAsync(Expression<Func<TModel, bool>> predicate)
		{
			var entityPredicate = _mapper.MapExpression<Expression<Func<TEntity, bool>>>(predicate);
			var entity = await _dbSet.FirstOrDefaultAsync(entityPredicate);
			return entity == null ? null : _mapper.Map<TModel>(entity);
		}

		/// <inheritdoc/>
		public virtual async Task AddAsync(TModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			var entity = _mapper.Map<TEntity>(model);
			await _dbSet.AddAsync(entity);
		}

		/// <inheritdoc/>
		public virtual async Task AddRangeAsync(IEnumerable<TModel> models)
		{
			if (models == null)
				throw new ArgumentNullException(nameof(models));

			var entities = _mapper.Map<IEnumerable<TEntity>>(models);
			await _dbSet.AddRangeAsync(entities);
		}

		/// <inheritdoc/>
		public virtual void Update(TModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			var entity = _mapper.Map<TEntity>(model);
			_dbSet.Update(entity);
		}

		/// <inheritdoc/>
		public virtual void Remove(TModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			var entity = _mapper.Map<TEntity>(model);
			_dbSet.Remove(entity);
		}

		/// <inheritdoc/>
		public virtual void RemoveRange(IEnumerable<TModel> models)
		{
			if (models == null)
				throw new ArgumentNullException(nameof(models));

			var entities = _mapper.Map<IEnumerable<TEntity>>(models);
			_dbSet.RemoveRange(entities);
		}

		/// <inheritdoc/>
		public virtual async Task<bool> ExistsAsync(Expression<Func<TModel, bool>> predicate)
		{
			var entityPredicate = _mapper.MapExpression<Expression<Func<TEntity, bool>>>(predicate);
			return await _dbSet.AnyAsync(entityPredicate);
		}

		/// <inheritdoc/>
		public virtual async Task<int> CountAsync()
		{
			return await _dbSet.CountAsync();
		}

		/// <inheritdoc/>
		public virtual async Task<int> CountAsync(Expression<Func<TModel, bool>> predicate)
		{
			var entityPredicate = _mapper.MapExpression<Expression<Func<TEntity, bool>>>(predicate);
			return await _dbSet.CountAsync(entityPredicate);
		}

		/// <inheritdoc/>
		public virtual async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		#endregion IRepository<TModel> Implementation

		#region Protected Methods

		/// <summary>
		/// Преобразует коллекцию сущностей в коллекцию моделей.
		/// </summary>
		/// <param name="entities">Коллекция сущностей</param>
		/// <returns>Коллекция моделей</returns>
		protected IEnumerable<TModel> MapEntitiesToModels(IEnumerable<TEntity> entities)
		{
			return entities.Select(entity => _mapper.Map<TModel>(entity));
		}

		/// <summary>
		/// Преобразует сущность в модель.
		/// </summary>
		/// <param name="entity">Сущность</param>
		/// <returns>Модель</returns>
		protected TModel? MapEntityToModel(TEntity? entity)
		{
			return entity == null ? null : _mapper.Map<TModel>(entity);
		}

		/// <summary>
		/// Преобразует модель в сущность.
		/// </summary>
		/// <param name="model">Модель</param>
		/// <returns>Сущность</returns>
		protected TEntity MapModelToEntity(TModel model)
		{
			return _mapper.Map<TEntity>(model);
		}

		/// <summary>
		/// Преобразует предикат для модели в предикат для сущности.
		/// </summary>
		/// <param name="predicate">Предикат для модели</param>
		/// <returns>Предикат для сущности</returns>
		protected Expression<Func<TEntity, bool>> ConvertPredicate(Expression<Func<TModel, bool>> predicate)
		{
			return _mapper.MapExpression<Expression<Func<TEntity, bool>>>(predicate);
		}

		#endregion Protected Methods
	}
}