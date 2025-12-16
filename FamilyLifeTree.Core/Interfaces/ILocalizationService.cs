namespace FamilyLifeTree.Core.Interfaces
{
#nullable enable

	/// <summary>
	/// Интерфейс сервиса локализации.
	/// </summary>
	public interface ILocalizationService
	{
		/// <summary>
		/// Получает локализованную строку.
		/// </summary>
		/// <param name="resourcesKey">Ключ ресурса локализации.</param>
		/// <param name="resourcesName">Имя ресурса откуда берется локализация.</param>
		public string GetLocalizationString(string resourcesKey, string? resourcesName = null);
	}
}
