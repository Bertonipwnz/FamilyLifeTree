namespace Utils.Serialization.Services
{
    using Serilog;
    using System;
    using System.Text.Json;
    using Utils.Logger;
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

		/// <summary>
		/// Логгер.
		/// </summary>
		private readonly ILogger _logger = LogService.GetCurrentLogger();

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
			try
			{
				var result = JsonSerializer.Serialize(value, options ?? _defaultOptions);
				_logger?.Debug("Serialized {Type} to JSON: {Json}", typeof(T).Name, result);

				return result;
			}
			catch (Exception ex)
			{
				_logger?.Error(ex, "Failed to serialize {Type}", typeof(T).Name);
				throw;
			}
		}

		/// <inheritdoc />
		public string Serialize(object value, JsonSerializerOptions? options = null)
		{
			if (value is null)
			{
				return "null";
			}

			var type = value.GetType();

			try
			{
				var result = JsonSerializer.Serialize(value, type, options ?? _defaultOptions);
				_logger?.Debug("Serialized {Type} to JSON: {Json}", type.Name, result);
				
				return result;
			}
			catch (Exception ex)
			{
				_logger?.Error(ex, "Failed to serialize {Type}", type.Name);
				throw;
			}
		}

		/// <inheritdoc />
		public T? Deserialize<T>(string json, JsonSerializerOptions? options = null)
		{
			try
			{
				var result = JsonSerializer.Deserialize<T>(json, options ?? _defaultOptions);
				_logger?.Debug("Deserialized JSON to {Type}: {Json}", typeof(T).Name, json);
				
				return result;
			}
			catch (Exception ex)
			{
				_logger?.Error(ex, "Failed to deserialize JSON to {Type}: {Json}", typeof(T).Name, json);
				throw;
			}
		}

		/// <inheritdoc />
		public object? Deserialize(string json, Type returnType, JsonSerializerOptions? options = null)
		{
			if (returnType is null)
			{
				throw new ArgumentNullException(nameof(returnType));
			}

			try
			{
				var result = JsonSerializer.Deserialize(json, returnType, options ?? _defaultOptions);
				_logger?.Debug("Deserialized JSON to {Type}: {Json}", returnType.Name, json);
				
				return result;
			}
			catch (Exception ex)
			{
				_logger?.Error(ex, "Failed to deserialize JSON to {Type}: {Json}", returnType.Name, json);
				throw;
			}
		}

		#endregion
	}
}


