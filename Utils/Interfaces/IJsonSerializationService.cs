namespace Utils.Interfaces
{
	using System;
	using System.Text.Json;

#nullable enable

	/// <summary>
	/// Сервис сериализации и десериализации JSON на базе <see cref="JsonSerializer"/>.
	/// Платформо-независимый, может использоваться из ViewModel-ей (.NET 8, UWP и др.).
	/// </summary>
	public interface IJsonSerializationService
	{
		/// <summary>
		/// Сериализует объект в JSON-строку.
		/// </summary>
		/// <typeparam name="T">Тип объекта.</typeparam>
		/// <param name="value">Объект для сериализации.</param>
		/// <param name="options">Дополнительные настройки сериализации.</param>
		/// <returns>JSON-строка.</returns>
		string Serialize<T>(T value, JsonSerializerOptions? options = null);

		/// <summary>
		/// Сериализует объект неизвестного во время компиляции типа в JSON-строку.
		/// </summary>
		/// <param name="value">Объект для сериализации.</param>
		/// <param name="options">Дополнительные настройки сериализации.</param>
		/// <returns>JSON-строка.</returns>
		string Serialize(object value, JsonSerializerOptions? options = null);

		/// <summary>
		/// Десериализует JSON-строку в объект указанного типа.
		/// </summary>
		/// <typeparam name="T">Тип результата.</typeparam>
		/// <param name="json">JSON-строка.</param>
		/// <param name="options">Дополнительные настройки десериализации.</param>
		/// <returns>Десериализованный объект.</returns>
		T? Deserialize<T>(string json, JsonSerializerOptions? options = null);

		/// <summary>
		/// Десериализует JSON-строку в объект указанного типа во время выполнения.
		/// </summary>
		/// <param name="json">JSON-строка.</param>
		/// <param name="returnType">Тип результата.</param>
		/// <param name="options">Дополнительные настройки десериализации.</param>
		/// <returns>Десериализованный объект.</returns>
		object? Deserialize(string json, Type returnType, JsonSerializerOptions? options = null);
	}
}


