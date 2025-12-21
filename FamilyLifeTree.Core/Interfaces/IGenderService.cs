namespace FamilyLifeTree.Core.Interfaces
{
	using Utils.Interfaces;

	/// <summary>
	/// Интерфейса сервиса гендеров.
	/// </summary>
	/// <typeparam name="M">Модель.</typeparam>
	/// <typeparam name="VM">Модель представления.</typeparam>
	public interface IGenderService<M, VM> : IEntityService<M, VM>, IAsyncInitializable
	{
	}
}
