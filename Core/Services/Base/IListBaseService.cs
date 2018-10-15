using System.Threading.Tasks;

namespace Core.Services.Base
{
    /// <summary>
    /// Interface décrivant les services de type list
    /// </summary>
    /// <typeparam name="TDomainList">Le type d'objet Domain</typeparam>
    /// <typeparam name="TFk">Clé etrangere de filtrage</typeparam>
    public interface IListBaseService<TDomainList, TFk> : IBaseService
    where TFk : struct
    {
        Task<TDomainList[]> List(out int total, TFk? foreignKey, int currentPage, int rowCount, string sort, string searchPhrase);

        Task<TDomainList[]> All(TFk? foreignKey, string sort = null, string query = null);
    }
}
