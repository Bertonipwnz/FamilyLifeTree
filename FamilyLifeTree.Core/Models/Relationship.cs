namespace FamilyLifeTree.Core.Models
{
	using FamilyLifeTree.Core.Enums;
	using System;

	/// <summary>
	/// Модель связи между персонами в семейном древе.
	/// </summary>
	public class Relationship
	{
		/// <summary>
		/// Уникальный идентификатор связи.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Тип родственной связи.
		/// </summary>
		public RelationshipType RelationshipType { get; set; }

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
		/// Длительность связи в годах (вычисляемое свойство).
		/// </summary>
		public double? DurationYears
		{
			get
			{
				if (!DateFormed.HasValue) 
					return null;

				var endDate = DateEnded ?? DateTime.Now;
				var duration = (endDate - DateFormed.Value).TotalDays / 365.25;

				return duration >= 0 ? Math.Round(duration, 1) : null;
			}
		}

		/// <summary>
		/// Активна ли связь в данный момент.
		/// </summary>
		public bool IsActive => !DateEnded.HasValue || DateEnded.Value > DateTime.Now;
	}
}