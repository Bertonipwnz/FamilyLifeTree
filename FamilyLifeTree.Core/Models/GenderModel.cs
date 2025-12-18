namespace FamilyLifeTree.Core.Models
{
    using FamilyLifeTree.Core.Enums;

    //TODO: Посмотреть необходимость public класса.
    /// <summary>
    /// Модель гендера.
    /// </summary>
    public class GenderModel
	{
		#region Public Properties

        /// <summary>
        /// Перечисление гендера.
        /// </summary>
		public Gender Gender { get; set; }

        /// <summary>
        /// Путь к иконке.
        /// </summary>
        public string IconPath { get; set; } = string.Empty;

		#endregion
	}
}
