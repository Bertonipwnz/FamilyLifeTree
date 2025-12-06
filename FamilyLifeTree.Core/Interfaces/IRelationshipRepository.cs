namespace FamilyLifeTree.Core.Interfaces
{
	using FamilyLifeTree.Core.Models;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Интерфейс репозитория для работы со связями между персонами.
	/// </summary>
	public interface IRelationshipRepository : IRepository<Relationship>
	{
		/// <summary>
		/// Получить все связи для указанной персоны.
		/// </summary>
		/// <param name="personId">Идентификатор персоны</param>
		/// <returns>Коллекция связей</returns>
		Task<IEnumerable<Relationship>> GetRelationshipsForPersonAsync(int personId);

		/// <summary>
		/// Получить связи между двумя персонами.
		/// </summary>
		/// <param name="personId1">Идентификатор первой персоны</param>
		/// <param name="personId2">Идентификатор второй персоны</param>
		/// <returns>Коллекция связей</returns>
		Task<IEnumerable<Relationship>> GetRelationshipsBetweenAsync(int personId1, int personId2);

		/// <summary>
		/// Проверить существование связи между персонами.
		/// </summary>
		/// <param name="primaryPersonId">Идентификатор основной персоны</param>
		/// <param name="relatedPersonId">Идентификатор связанной персоны</param>
		/// <param name="relationshipType">Тип связи</param>
		/// <returns>True, если связь существует</returns>
		Task<bool> RelationshipExistsAsync(int primaryPersonId, int relatedPersonId, Enums.RelationshipType relationshipType);

		/// <summary>
		/// Получить связанных персон для указанной персоны.
		/// </summary>
		/// <param name="personId">Идентификатор персоны</param>
		/// <param name="relationshipType">Тип связи (если null - все типы)</param>
		/// <returns>Коллекция связанных персон</returns>
		Task<IEnumerable<Person>> GetRelatedPersonsAsync(int personId, Enums.RelationshipType? relationshipType = null);
	}
}
