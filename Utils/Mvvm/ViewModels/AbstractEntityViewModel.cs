namespace Utils.Mvvm.ViewModels
{
	using CommunityToolkit.Mvvm.ComponentModel;
	
	/// <summary>
	/// Абстрактная модель представления сущности.
	/// </summary>
	/// <typeparam name="T">Модель сущности.</typeparam>
	public abstract class AbstractEntityViewModel<T> : ObservableObject
		where T : class
	{
		#region Public Constructors

		/// <summary>
		/// Создает экземпляр <see cref="AbstractEntityViewModel{T}"/>
		/// </summary>
		/// <param name="model">Модель.</param>
		public AbstractEntityViewModel(T model)
		{

		}

		#endregion
	}
}
