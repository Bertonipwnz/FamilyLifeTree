namespace Utils.Interfaces
{
	/// <summary>
	/// Интерфейс сущности которую можно перемещать.
	/// </summary>
	public interface IMoveable
	{
		/// <summary>
		/// Позиция по X.
		/// </summary>
		public double X { get; set; }

		/// <summary>
		/// Позиция по Y.
		/// </summary>
		public double Y { get; set; }
	}
}
