namespace FamilyLifeTree.DataAccess.Repositories
{
	using AutoMapper;
	using FamilyLifeTree.Core.Enums;
	using FamilyLifeTree.Core.Interfaces;
	using FamilyLifeTree.Core.Models;
	using FamilyLifeTree.DataAccess.DbContext;
	using FamilyLifeTree.DataAccess.Entities;
	using FamilyLifeTree.DataAccess.Repositories.Base;
	using Microsoft.EntityFrameworkCore;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

#nullable enable

	/// <summary>
	/// Репозиторий для работы со связями между персонами.
	/// </summary>
	public class RelationshipRepository : BaseRepository<RelationshipEntity, Relationship>, IRelationshipRepository
	{
		#region Constructor

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="RelationshipRepository"/>.
		/// </summary>
		/// <param name="context">Контекст базы данных</param>
		/// <param name="mapper">Маппер для преобразования сущностей</param>
		public RelationshipRepository(FamilyTreeDbContext context, IMapper mapper)
			: base(context, mapper)
		{
		}

		#endregion Constructor

		#region IRelationshipRepository Implementation

		/// <inheritdoc/>
		public async Task<IEnumerable<Relationship>> GetRelationshipsForPersonAsync(int personId)
		{
			var entities = await _context.Relationships
				.Include(r => r.PrimaryPerson)
				.Include(r => r.RelatedPerson)
				.Where(r => r.PrimaryPersonId == personId || r.RelatedPersonId == personId)
				.OrderBy(r => r.RelationshipType)
				.ThenBy(r => r.DateFormed)
				.ToListAsync();

			return MapEntitiesToModels(entities);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Relationship>> GetRelationshipsBetweenAsync(int personId1, int personId2)
		{
			var entities = await _context.Relationships
				.Include(r => r.PrimaryPerson)
				.Include(r => r.RelatedPerson)
				.Where(r => (r.PrimaryPersonId == personId1 && r.RelatedPersonId == personId2) ||
						   (r.PrimaryPersonId == personId2 && r.RelatedPersonId == personId1))
				.OrderBy(r => r.RelationshipType)
				.ToListAsync();

			return MapEntitiesToModels(entities);
		}

		/// <inheritdoc/>
		public async Task<bool> RelationshipExistsAsync(int primaryPersonId, int relatedPersonId, RelationshipType relationshipType)
		{
			return await _context.Relationships
				.AnyAsync(r => r.PrimaryPersonId == primaryPersonId &&
							  r.RelatedPersonId == relatedPersonId &&
							  r.RelationshipType == (int)relationshipType);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetRelatedPersonsAsync(int personId, RelationshipType? relationshipType = null)
		{
			var query = _context.Relationships
				.Include(r => r.PrimaryPerson)
				.Include(r => r.RelatedPerson)
				.Where(r => r.PrimaryPersonId == personId || r.RelatedPersonId == personId);

			if (relationshipType.HasValue)
			{
				query = query.Where(r => r.RelationshipType == (int)relationshipType.Value);
			}

			var entities = await query.ToListAsync();
			var relatedPersons = new List<Person>();

			foreach (var entity in entities)
			{
				if (entity.PrimaryPersonId == personId)
				{
					relatedPersons.Add(_mapper.Map<Person>(entity.RelatedPerson)!);
				}
				else
				{
					relatedPersons.Add(_mapper.Map<Person>(entity.PrimaryPerson)!);
				}

			}

			return relatedPersons.DistinctBy(p => p.Id);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Relationship>> GetByTypeAsync(RelationshipType relationshipType)
		{
			return await FindAsync(r => r.RelationshipType == relationshipType);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Relationship>> GetActiveRelationshipsAsync()
		{
			return await FindAsync(r => r.IsActive);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Relationship>> GetConfirmedRelationshipsAsync()
		{
			return await FindAsync(r => r.IsConfirmed);
		}

		/// <inheritdoc/>
		public async Task<bool> RemoveRelationshipAsync(int primaryPersonId, int relatedPersonId, RelationshipType relationshipType)
		{
			var entity = await _context.Relationships
				.FirstOrDefaultAsync(r => r.PrimaryPersonId == primaryPersonId &&
										 r.RelatedPersonId == relatedPersonId &&
										 r.RelationshipType == (int)relationshipType);

			if (entity == null)
				return false;

			_context.Relationships.Remove(entity);
			await _context.SaveChangesAsync();

			return true;
		}

		/// <inheritdoc/>
		public async Task<Relationship?> GetRelationshipWithPersonsAsync(int relationshipId)
		{
			var entity = await _context.Relationships
				.Include(r => r.PrimaryPerson)
				.Include(r => r.RelatedPerson)
				.FirstOrDefaultAsync(r => r.Id == relationshipId);

			return MapEntityToModel(entity);
		}

		/// <inheritdoc/>
		public async Task<bool> ExistsAsync(int id)
		{
			return await _dbSet.AnyAsync(e => e.Id == id);
		}

		/// <inheritdoc/>
		public async Task<bool> SafeRemoveAsync(int id)
		{
			var entity = await _dbSet.FindAsync(id);
			if (entity == null)
				return false;

			// Проверяем, не является ли связь критически важной
			// Например, связь родитель-ребенок не должна удаляться без проверки
			var isCritical = entity.RelationshipType == (int)RelationshipType.Parent ||
							entity.RelationshipType == (int)RelationshipType.Child;

			if (isCritical)
			{
				if (entity.RelationshipType == (int)RelationshipType.Parent)
				{
					var childHasOtherParents = await _context.Relationships
						.AnyAsync(r => r.RelatedPersonId == entity.RelatedPersonId &&
									  r.RelationshipType == (int)RelationshipType.Parent &&
									  r.Id != id);

					if (!childHasOtherParents)
						return false;
				}
			}

			_dbSet.Remove(entity);
			await _context.SaveChangesAsync();

			return true;
		}

		#endregion IRelationshipRepository Implementation

		#region Public Methods

		/// <summary>
		/// Получает все связи определенного типа для указанной персоны.
		/// </summary>
		public async Task<IEnumerable<Relationship>> GetRelationshipsByTypeForPersonAsync(int personId, RelationshipType relationshipType)
		{
			var entities = await _context.Relationships
				.Include(r => r.PrimaryPerson)
				.Include(r => r.RelatedPerson)
				.Where(r => (r.PrimaryPersonId == personId || r.RelatedPersonId == personId) &&
						   r.RelationshipType == (int)relationshipType)
				.ToListAsync();

			return MapEntitiesToModels(entities);
		}

		/// <summary>
		/// Проверяет, существует ли связь между двумя персонами (любого типа).
		/// </summary>
		public async Task<bool> AnyRelationshipExistsAsync(int personId1, int personId2)
		{
			return await _context.Relationships
				.AnyAsync(r => (r.PrimaryPersonId == personId1 && r.RelatedPersonId == personId2) ||
							  (r.PrimaryPersonId == personId2 && r.RelatedPersonId == personId1));
		}

		#endregion Public Methods
	}
}