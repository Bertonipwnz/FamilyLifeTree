namespace FamilyLifeTree.DataAccess.Entities.Base
{
	using System;

	/// <summary>
	/// Базовая сущность с общими свойствами.
	/// </summary>
	public abstract class BaseEntity
	{
		/// <summary>
		/// Идентификатор.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Дата создания.
		/// </summary>
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		/// <summary>
		/// Дата обновления.
		/// </summary>
		public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
	}
}
