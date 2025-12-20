namespace Utils.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс сервиса сущности.
    /// </summary>
    /// <typeparam name="M">Модель.</typeparam>
    /// <typeparam name="VM">Модель представления.</typeparam>
    public interface IEntityService<M,VM>
    {
        /// <summary>
        /// Создает модель
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public VM CreateVM(M model);

        /// <summary>
        /// Метод получения поделей.
        /// </summary>
        public async Task<List<M>> GetModelsAsync();

    }
}
