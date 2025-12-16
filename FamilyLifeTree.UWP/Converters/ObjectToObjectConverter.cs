namespace FamilyLifeTree.UWP.Converters
{
	using System;
	using Windows.UI.Xaml.Data;

	/// <summary>
	/// Универсальный конвертер object → object по совпадению значения.
	/// Если value.Equals(TrueValue), возвращает TrueResult.
	/// Иначе — FalseResult.
	/// </summary>
	public sealed class ObjectToObjectConverter : IValueConverter
	{
		/// <summary>
		/// Значение для сравнения (аналог "true").
		/// </summary>
		public object TrueValue { get; set; } = null!;

		/// <summary>
		/// Результат при совпадении с TrueValue.
		/// </summary>
		public object TrueResult { get; set; } = null!;

		/// <summary>
		/// Результат при несовпадении.
		/// </summary>
		public object FalseResult { get; set; } = null!;

		/// <inheritdoc/>
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			return value?.Equals(TrueValue) == true ? TrueResult : FalseResult;
		}

		/// <inheritdoc/>
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			return value?.Equals(TrueResult) == true ? TrueValue : FalseResult;
		}
	}
}
