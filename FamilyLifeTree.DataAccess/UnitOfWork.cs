namespace FamilyLifeTree.DataAccess
{
	using AutoMapper;
	using FamilyLifeTree.Core.Interfaces;
	using FamilyLifeTree.DataAccess.DbContext;
	using FamilyLifeTree.DataAccess.Repositories;
	using Microsoft.EntityFrameworkCore.Storage;
	using System;
	using System.Threading.Tasks;

#nullable enable

	/// <summary>
	/// Unit of Work для координации работы репозиториев и управления транзакциями.
	/// </summary>
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		#region Private Fields

		/// <summary>
		/// Контекст БД.
		/// </summary>
		private readonly FamilyTreeDbContext _context;

		/// <summary>
		/// Маппер.
		/// </summary>
		private readonly IMapper _mapper;

		/// <summary>
		/// Текущая транзакция.
		/// </summary>
		private IDbContextTransaction? _currentTransaction;

		/// <summary>
		/// Уничтожен ли класс?
		/// </summary>
		private bool _disposed;

		/// <summary>
		/// <inheritdoc cref="Relationships"/>
		/// </summary>
		private readonly Lazy<IRelationshipRepository> _relationships;

		/// <summary>
		/// <inheritdoc cref="Persons"/>
		/// </summary>
		private readonly Lazy<IPersonRepository> _persons;

		#endregion Private Fields

		#region Public Properties

		/// <inheritdoc/>
		public IPersonRepository Persons => _persons.Value;

		/// <inheritdoc/>
		public IRelationshipRepository Relationships => _relationships.Value;

		#endregion Public Properties

		#region Constructor

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="UnitOfWork"/>.
		/// </summary>
		/// <param name="context">Контекст базы данных</param>
		/// <param name="mapper">Маппер для преобразования сущностей</param>
		/// <exception cref="ArgumentNullException">Выбрасывается, если context или mapper равны null</exception>
		public UnitOfWork(FamilyTreeDbContext context, IMapper mapper)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

			_persons = new Lazy<IPersonRepository>(() => new PersonRepository(_context, _mapper));
			_relationships = new Lazy<IRelationshipRepository>(() => new RelationshipRepository(_context, _mapper));
		}

		#endregion Constructor

		#region Public Methods

		/// <inheritdoc/>
		public async Task<int> CompleteAsync()
		{
			try
			{
				return await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new InvalidOperationException("Ошибка при сохранении изменений в базе данных.", ex);
			}
		}

		/// <inheritdoc/>
		public async Task BeginTransactionAsync()
		{
			if (_currentTransaction != null)
			{
				throw new InvalidOperationException("Транзакция уже начата.");
			}

			_currentTransaction = await _context.Database.BeginTransactionAsync();
		}

		/// <inheritdoc/>
		public async Task CommitTransactionAsync()
		{
			if (_currentTransaction == null)
			{
				throw new InvalidOperationException("Нет активной транзакции для подтверждения.");
			}

			try
			{
				await _context.SaveChangesAsync();
				await _currentTransaction.CommitAsync();
			}
			catch
			{
				await RollbackTransactionAsync();
				throw;
			}
			finally
			{
				await DisposeTransactionAsync();
			}
		}

		/// <inheritdoc/>
		public async Task RollbackTransactionAsync()
		{
			if (_currentTransaction == null)
			{
				throw new InvalidOperationException("Нет активной транзакции для отката.");
			}

			try
			{
				await _currentTransaction.RollbackAsync();
			}
			finally
			{
				await DisposeTransactionAsync();
			}
		}

		/// <summary>
		/// Освобождает ресурсы, используемые экземпляром <see cref="UnitOfWork"/>.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Финализатор для гарантированного освобождения ресурсов.
		/// </summary>
		~UnitOfWork()
		{
			Dispose(false);
		}

		#endregion Public Methods

		#region Protected Methods

		/// <summary>
		/// Освобождает неуправляемые ресурсы и при необходимости освобождает управляемые ресурсы.
		/// </summary>
		/// <param name="disposing">Значение true позволяет освободить управляемые и неуправляемые ресурсы; значение false освобождает только неуправляемые ресурсы.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					DisposeTransactionAsync().GetAwaiter().GetResult();
					_context.Dispose();
				}

				_disposed = true;
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Освобождает текущую транзакцию, если она существует.
		/// </summary>
		private async Task DisposeTransactionAsync()
		{
			if (_currentTransaction != null)
			{
				await _currentTransaction.DisposeAsync();
				_currentTransaction = null;
			}
		}

		#endregion Private Methods
	}
}