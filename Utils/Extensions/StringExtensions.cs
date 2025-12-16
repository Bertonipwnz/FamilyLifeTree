namespace Utils.Extensions
{
	using System.Text.RegularExpressions;

	/// <summary>
	/// Расширение для string.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Регулярное выражене для валидации имени.
		/// </summary>
		private static readonly Regex _nameRegex = new(@"^[\p{L}\p{M}]+(?:[ -\p{Pd}][\p{L}\p{M}]+)*$", RegexOptions.Compiled);

		/// <summary>
		/// Проверяет соддержит строку символы для имени.
		/// </summary>
		/// <param name="inputString">Входящая строка.</param>
		public static bool IsLettersOnlyName(this string inputString)
		{
			return !string.IsNullOrWhiteSpace(inputString) && _nameRegex.IsMatch(inputString.Trim());
		}
	}
}
