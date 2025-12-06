namespace FamilyLifeTree.DataAccess.Entities
{
    using FamilyLifeTree.DataAccess.Entities.Base;
    using System;

#nullable enable

    /// <summary>
    /// Сущность связи между персонами для EF.
    /// Содержит только данные для хранения в БД.
    /// </summary>
    public class RelationshipEntity : BaseEntity
	{
		/// <summary>
		/// Тип родственной связи.
		/// Хранится как int в БД, соответствует enum RelationshipType.
		/// </summary>
		public int RelationshipType { get; set; }

		/// <summary>
		/// Дата установления связи (свадьба, рождение, усыновление и т.д.).
		/// </summary>
		public DateTime? DateFormed { get; set; }

		/// <summary>
		/// Дата прекращения связи (развод, смерть и т.д.).
		/// </summary>
		public DateTime? DateEnded { get; set; }

		/// <summary>
		/// Примечания к связи.
		/// </summary>
		public string? Notes { get; set; }

		/// <summary>
		/// Идентификатор основной персоны (инициатор связи).
		/// </summary>
		public int PrimaryPersonId { get; set; }

		/// <summary>
		/// Идентификатор связанной персоны (цель связи).
		/// </summary>
		public int RelatedPersonId { get; set; }

		/// <summary>
		/// Метка подтверждения связи (подтверждена ли связь пользователем).
		/// </summary>
		public bool IsConfirmed { get; set; } = true;

		/// <summary>
		/// Причина, по которой связь не подтверждена.
		/// </summary>
		public string? ConfirmationReason { get; set; }

		/// <summary>
		/// Навигационное свойство - основная персона (инициатор связи).
		/// Используется Entity Framework для ленивой загрузки связанных данных.
		/// Virtual требуется для работы механизма прокси EF Core.
		/// </summary>
		/// <remarks>
		/// Пример: если Алексей является родителем Марии, то Алексей - PrimaryPerson.
		/// </remarks>
		public virtual PersonEntity? PrimaryPerson { get; set; }

		/// <summary>
		/// Навигационное свойство - связанная персона (цель связи).
		/// Используется Entity Framework для ленивой загрузки связанных данных.
		/// Virtual требуется для работы механизма прокси EF Core.
		/// </summary>
		/// <remarks>
		/// Пример: если Алексей является родителем Марии, то Мария - RelatedPerson.
		/// </remarks>
		public virtual PersonEntity? RelatedPerson { get; set; }
	}
}
