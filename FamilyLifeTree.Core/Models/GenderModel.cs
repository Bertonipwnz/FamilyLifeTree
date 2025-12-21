namespace FamilyLifeTree.Core.Models
{
	using FamilyLifeTree.Core.Enums;
	using System.Text.Json.Serialization;

	/// <summary>
	/// Модель гендера.
	/// </summary>
	public class GenderModel
	{
		#region Public Properties

		/// <summary>
		/// Перечисление гендера.
		/// </summary>
		[JsonPropertyName("gender")]
		public Gender Gender { get; set; }

		/// <summary>
		/// Путь к иконке.
		/// </summary>
		[JsonPropertyName("icon_path")]
		public string IconPath { get; set; } = string.Empty;

		#endregion
	}
}
