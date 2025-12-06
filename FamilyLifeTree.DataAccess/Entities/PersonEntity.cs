namespace FamilyLifeTree.DataAccess.Entities
{
    using FamilyLifeTree.DataAccess.Entities.Base;
    using System;
    using System.Collections.Generic;

#nullable enable

    /// <summary>
    /// Сущность персоны для EF.
    /// </summary>
    public class PersonEntity : BaseEntity
	{
		/// <summary>
		/// Имя.
		/// </summary>
		public string FirstName { get; set; } = string.Empty;

		/// <summary>
		/// Фамилия.
		/// </summary>
		public string LastName { get; set; } = string.Empty;

		/// <summary>
		/// Отчество.
		/// </summary>
		public string? MiddleName { get; set; }

		/// <summary>
		/// Дата рождения.
		/// </summary>
		public DateTime? BirthDate { get; set; }

		/// <summary>
		/// Дата смерти.
		/// </summary>
		public DateTime? DeathDate { get; set; }

		/// <summary>
		/// Гендер. Хранится как int в БД, соответствует enum Gender.
		/// </summary>
		public int Gender { get; set; }

		/// <summary>
		/// Биография.
		/// </summary>
		public string? Biography { get; set; }

		/// <summary>
		/// Путь к фото.
		/// </summary>
		public string? PhotoPath { get; set; }

		/// <summary>
		/// Навигационное свойство. Связи, где эта персона является основной (инициатором связи).
		/// Например: если Алексей является родителем Марии, то связь "Parent" будет в коллекции RelationshipsAsPrimary у Алексея.
		/// </summary>
		/// <remarks>
		/// Используется Entity Framework для ленивой загрузки связанных данных.
		/// Virtual требуется для работы механизма прокси EF Core.
		/// </remarks>
		public virtual ICollection<RelationshipEntity> RelationshipsAsPrimary { get; set; }
			= new List<RelationshipEntity>();

		/// <summary>
		/// Навигационное свойство. Связи, где эта персона является связанной (целью связи).
		/// Например: если Алексей является родителем Марии, то связь "Child" будет в коллекции RelationshipsAsRelated у Марии.
		/// </summary>
		/// <remarks>
		/// Обратная сторона связи. Позволяет получить все связи, где данная персона упоминается как связанная.
		/// </remarks>
		public virtual ICollection<RelationshipEntity> RelationshipsAsRelated { get; set; }
			= new List<RelationshipEntity>();
	}
}
