namespace Utils.Serialization.Services
{
    using System;
    using System.Text.Json;
    using Utils.Serialization.Services.Interfaces;

#nullable enable

    /// <summary>
    /// Реализация <see cref="IJsonSerializationService"/> на базе <see cref="JsonSerializer"/>.
    /// </summary>
    public class JsonSerializationService : IJsonSerializationService
	{
		#region Private Fields

		/// <summary>
		/// Дефолтные опции.
		/// </summary>
		private readonly JsonSerializerOptions _defaultOptions;

		#endregion

		#region Public Methods

		/// <summary>
		/// Инициализирует новый экземпляр класса <see cref="JsonSerializationService"/>.
		/// </summary>
		/// <param name="defaultOptions">
		/// Базовые настройки сериализации/десериализации.
		/// Если не указаны, используются разумные значения по умолчанию (camelCase и т.д.).
		/// </param>
		public JsonSerializationService(JsonSerializerOptions? defaultOptions = null)
		{
			_defaultOptions = defaultOptions ?? new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
				WriteIndented = false
			};
		}

		/// <inheritdoc />
		public string Serialize<T>(T value, JsonSerializerOptions? options = null)
		{
			return JsonSerializer.Serialize(value, options ?? _defaultOptions);
		}

		/// <inheritdoc />
		public string Serialize(object value, JsonSerializerOptions? options = null)
		{
			if (value is null)
			{
				return "null";
			}

			return JsonSerializer.Serialize(value, value.GetType(), options ?? _defaultOptions);
		}

		/// <inheritdoc />
		public T? Deserialize<T>(string json, JsonSerializerOptions? options = null)
		{
			return JsonSerializer.Deserialize<T>(json, options ?? _defaultOptions);
		}

		/// <inheritdoc />
		public object? Deserialize(string json, Type returnType, JsonSerializerOptions? options = null)
		{
			if (returnType is null)
			{
				throw new ArgumentNullException(nameof(returnType));
			}

			return JsonSerializer.Deserialize(json, returnType, options ?? _defaultOptions);
		}

		#endregion
	}
}


