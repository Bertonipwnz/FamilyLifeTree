namespace FamilyLifeTree.DataAccess.Entities
{
    using FamilyLifeTree.DataAccess.Entities.Base;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Сущность персоны для EF.
    /// </summary>
    [Table("Persons")]
	public class PersonEntity : BaseEntity
	{
		/// <summary>
		/// Имя.
		/// </summary>
		[Required]
		[MaxLength(100)]
		public string FirstName { get; set; } = string.Empty;

		/// <summary>
		/// Фамилия.
		/// </summary>
		[Required]
		[MaxLength(100)]
		public string LastName { get; set; } = string.Empty;

		/// <summary>
		/// Отчество.
		/// </summary>
		[MaxLength(100)]
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
		/// Гендер.
		/// </summary>
		[Required]
		public int Gender { get; set; }

		/// <summary>
		/// Биография.
		/// </summary>
		[MaxLength(2000)]
		public string? Biography { get; set; }

		/// <summary>
		/// Фото.
		/// </summary>
		[MaxLength(500)]
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
