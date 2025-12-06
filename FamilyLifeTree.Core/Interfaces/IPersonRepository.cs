namespace FamilyLifeTree.Core.Interfaces
{
	using FamilyLifeTree.Core.Models;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	/// <summary>
	/// Интерфейс репозитория для работы с персонами.
	/// </summary>
	public interface IPersonRepository : IRepository<Person>
	{
		/// <summary>
		/// Поиск персон по имени, фамилии или отчеству.
		/// </summary>
		/// <param name="searchTerm">Строка для поиска</param>
		/// <returns>Коллекция найденных персон</returns>
		Task<IEnumerable<Person>> SearchByNameAsync(string searchTerm);

		/// <summary>
		/// Получить всех живых персон.
		/// </summary>
		Task<IEnumerable<Person>> GetAlivePersonsAsync();

		/// <summary>
		/// Получить персон по фамилии.
		/// </summary>
		Task<IEnumerable<Person>> GetByLastNameAsync(string lastName);

		/// <summary>
		/// Получить персону с её связями.
		/// </summary>
		Task<Person?> GetPersonWithRelationshipsAsync(int personId);

		/// <summary>
		/// Получить всех персон с их связями.
		/// </summary>
		Task<IEnumerable<Person>> GetAllWithRelationshipsAsync();
	}
}
