namespace Web_253505_Tarhonski.Sevices.AirsoftService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::Web_253505_Tarhonski.Domain.Entities;
    using global::Web_253505_Tarhonski.Domain.Models;
    using Microsoft.AspNetCore.Http;

    namespace Web_253505_Tarhonski.UI.Services.AirsoftService
    {
        /// <summary>
        /// Интерфейс для работы с объектами Airsoft
        /// </summary>
        public interface IAirsoftService
        {
            /// <summary>
            /// Получение списка всех объектов
            /// </summary>
            /// <param name="categoryNormalizedName">Нормализованное имя категории для фильтрации</param>
            /// <param name="pageNo">Номер страницы списка</param>
            /// <returns>Ответ с данными списка объектов Airsoft</returns>
            Task<ResponseData<ListModel<Airsoft>>> GetAirsoftListAsync(string? categoryNormalizedName, int pageNo = 1);

            /// <summary>
            /// Поиск объекта по Id
            /// </summary>
            /// <param name="id">Идентификатор объекта</param>
            /// <returns>Найденный объект или null, если объект не найден</returns>
            Task<ResponseData<Airsoft>> GetAirsoftByIdAsync(Guid id);
            Task<ResponseData<ListModel<Airsoft>>> GetAllAirsoftsAsync(string? categoryNormalizedName);

            /// <summary>
            /// Обновление объекта
            /// </summary>
            /// <param name="id">Id изменяемого объекта</param>
            /// <param name="airsoft">Объект с новыми параметрами</param>
            /// <param name="formFile">Файл изображения</param>
            /// <returns></returns>
            Task UpdateAirsoftAsync(Guid id, Airsoft airsoft, IFormFile? formFile);

            /// <summary>
            /// Удаление объекта
            /// </summary>
            /// <param name="id">Id удаляемого объекта</param>
            /// <returns></returns>
            Task DeleteAirsoftAsync(Guid id);

            /// <summary>
            /// Создание объекта
            /// </summary>
            /// <param name="airsoft">Новый объект</param>
            /// <param name="formFile">Файл изображения</param>
            /// <returns>Созданный объект</returns>
            Task<ResponseData<Airsoft>> CreateAirsoftAsync(Airsoft airsoft, IFormFile? formFile);
        }
    }

}
    