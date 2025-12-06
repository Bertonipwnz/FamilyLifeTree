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
	/// Репозиторий для работы с персонами.
	/// </summary>
	public class PersonRepository : BaseRepository<PersonEntity, Person>, IPersonRepository
	{
		#region Constructor

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="PersonRepository"/>.
		/// </summary>
		/// <param name="context">Контекст базы данных</param>
		/// <param name="mapper">Маппер для преобразования сущностей</param>
		public PersonRepository(FamilyTreeDbContext context, IMapper mapper)
			: base(context, mapper)
		{
		}

		#endregion Constructor

		#region IPersonRepository Implementation

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> SearchByNameAsync(string searchTerm)
		{
			if (string.IsNullOrWhiteSpace(searchTerm))
				return Enumerable.Empty<Person>();

			// Используем Expression для поиска по имени
			return await FindAsync(p =>
				p.FirstName.Contains(searchTerm) ||
				p.LastName.Contains(searchTerm) ||
				(!string.IsNullOrEmpty(p.MiddleName) && p.MiddleName.Contains(searchTerm)));
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetAlivePersonsAsync()
		{
			// Используем Expression для фильтрации живых персон
			return await FindAsync(p => p.IsAlive);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetByLastNameAsync(string lastName)
		{
			if (string.IsNullOrWhiteSpace(lastName))
				return Enumerable.Empty<Person>();

			return await FindAsync(p => p.LastName == lastName);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetByFullNameAsync(string firstName, string lastName)
		{
			if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
				return Enumerable.Empty<Person>();

			return await FindAsync(p =>
				p.FirstName == firstName && p.LastName == lastName);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetByBirthYearAsync(int year)
		{
			return await FindAsync(p =>
				p.BirthDate.HasValue && p.BirthDate.Value.Year == year);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetByGenderAsync(Gender gender)
		{
			return await FindAsync(p => p.Gender == gender);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetParentsAsync(int personId)
		{
			// Находим связи типа "родитель", где указанный человек является ребенком
			var relationships = await _context.Relationships
				.Include(r => r.PrimaryPerson) // Загружаем информацию о родителе
				.Where(r => r.RelatedPersonId == personId &&
						   r.RelationshipType == (int)RelationshipType.Parent)
				.ToListAsync();

			return relationships.Select(r => MapEntityToModel(r.PrimaryPerson)!);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetChildrenAsync(int personId)
		{
			// Находим связи типа "ребенок", где указанный человек является родителем
			var relationships = await _context.Relationships
				.Include(r => r.RelatedPerson) // Загружаем информацию о ребенке
				.Where(r => r.PrimaryPersonId == personId &&
						   r.RelationshipType == (int)RelationshipType.Child)
				.ToListAsync();

			return relationships.Select(r => MapEntityToModel(r.RelatedPerson)!);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetSpousesAsync(int personId)
		{
			// Находим связи типа "супруг/супруга" или "партнер"
			var relationships = await _context.Relationships
				.Include(r => r.PrimaryPerson)
				.Include(r => r.RelatedPerson)
				.Where(r => (r.PrimaryPersonId == personId || r.RelatedPersonId == personId) &&
						   (r.RelationshipType == (int)RelationshipType.Spouse ||
							r.RelationshipType == (int)RelationshipType.Partner))
				.ToListAsync();

			// Определяем, кто является супругом/партнером
			var spouses = relationships.Select(r =>
				r.PrimaryPersonId == personId ?
					MapEntityToModel(r.RelatedPerson)! :
					MapEntityToModel(r.PrimaryPerson)!);

			return spouses;
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetSiblingsAsync(int personId)
		{
			// Сначала находим родителей человека
			var parents = await GetParentsAsync(personId);
			var parentIds = parents.Select(p => p.Id).ToList();

			if (!parentIds.Any())
				return Enumerable.Empty<Person>();

			// Находим всех детей этих родителей (братьев и сестер)
			// Исключаем самого человека из результатов
			var siblings = new List<Person>();

			foreach (var parentId in parentIds)
			{
				var children = await GetChildrenAsync(parentId);
				siblings.AddRange(children.Where(c => c.Id != personId));
			}

			// Убираем дубликаты (если у братьев/сестер оба родителя одинаковые)
			return siblings.DistinctBy(p => p.Id);
		}

		/// <inheritdoc/>
		public async Task<Person?> GetPersonWithRelationshipsAsync(int personId)
		{
			var entity = await _context.Persons
				.Include(p => p.RelationshipsAsPrimary)
					.ThenInclude(r => r.RelatedPerson)
				.Include(p => p.RelationshipsAsRelated)
					.ThenInclude(r => r.PrimaryPerson)
				.FirstOrDefaultAsync(p => p.Id == personId);

			return MapEntityToModel(entity);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetAllWithRelationshipsAsync()
		{
			var entities = await _context.Persons
				.Include(p => p.RelationshipsAsPrimary)
				.Include(p => p.RelationshipsAsRelated)
				.OrderBy(p => p.LastName)
				.ThenBy(p => p.FirstName)
				.ToListAsync();

			return MapEntitiesToModels(entities);
		}

		/// <inheritdoc/>
		public async Task<IEnumerable<Person>> GetPagedAsync(int page, int pageSize)
		{
			if (page < 1) page = 1;
			if (pageSize < 1) pageSize = 10;

			var entities = await _dbSet
				.OrderBy(p => p.LastName)
				.ThenBy(p => p.FirstName)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();

			return MapEntitiesToModels(entities);
		}

		/// <inheritdoc/>
		public async Task<bool> SafeRemoveAsync(int id)
		{
			var entity = await _dbSet.FindAsync(id);
			if (entity == null)
				return false;

			// Проверяем, есть ли у человека важные связи
			var hasImportantRelationships = await _context.Relationships
				.AnyAsync(r => r.PrimaryPersonId == id || r.RelatedPersonId == id);

			if (hasImportantRelationships)
				return false; // Не удаляем, если есть связи

			_dbSet.Remove(entity);
			await _context.SaveChangesAsync();

			return true;
		}

		/// <inheritdoc/>
		public async Task<bool> ExistsAsync(int id)
		{
			return await _dbSet.AnyAsync(e => e.Id == id);
		}

		#endregion IPersonRepository Implementation
	}
}