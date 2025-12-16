namespace Utils.Extensions
{
	using System;

	/// <summary>
	/// Расширение для DateTimeOffset.
	/// </summary>
	public static class DateTimeOffsetExtensions
	{
		/// <summary>
		/// Валидна ли дата (не в будущем и не раньше другой даты).
		/// </summary>
		public static bool IsValidDate(this DateTimeOffset? date) =>
			!date.HasValue || date.Value.Date <= DateTimeOffset.Now;

		/// <summary>
		/// Валидна ли дата смерти?
		/// </summary>
		/// <param name="deathDate">Дата смерти.</param>
		/// <param name="birthDate">Дата рождения.</param>
		public static bool IsValidDeathDate(this DateTimeOffset? deathDate, DateTimeOffset? birthDate) =>
			deathDate.IsValidDate() &&
			(!deathDate.HasValue ||
			 (birthDate.HasValue && deathDate.Value.Date >= birthDate.Value.Date));
	}
}
