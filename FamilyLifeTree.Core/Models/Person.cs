namespace FamilyLifeTree.Core.Models
{
	using FamilyLifeTree.Core.Enums;
	using System;

	/// <summary>
	/// Модель персоны.
	/// </summary>
	public class Person
	{
		/// <summary>
		/// Айди в БД.
		/// </summary>
		public int Id { get; set; }

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
		/// Гендер.
		/// </summary>
		public Gender Gender { get; set; }

		/// <summary>
		/// Биография.
		/// </summary>
		public string? Biography { get; set; }

		/// <summary>
		/// Путь к фото.
		/// </summary>
		public string? PhotoPath { get; set; }

		/// <summary>
		/// Полное имя с отчеством.
		/// </summary>
		public string FullName
		{
			get
			{
				if (string.IsNullOrEmpty(MiddleName))
					return $"{FirstName} {LastName}";
				return $"{LastName} {FirstName} {MiddleName}";
			}
		}

		/// <summary>
		/// Короткое имя (Имя Фамилия).
		/// </summary>
		public string ShortName => $"{FirstName} {LastName}";

		/// <summary>
		/// Возраст (если жив) или возраст на момент смерти.
		/// </summary>
		public int? Age
		{
			get
			{
				if (!BirthDate.HasValue) return null;

				var endDate = DeathDate ?? DateTime.Today;
				var age = endDate.Year - BirthDate.Value.Year;

				// Корректировка если день рождения в этом году еще не наступил
				if (BirthDate.Value.Date > endDate.AddYears(-age))
					age--;

				return age;
			}
		}

		/// <summary>
		/// Жив ли человек.
		/// </summary>
		public bool IsAlive => !DeathDate.HasValue;
	}
}