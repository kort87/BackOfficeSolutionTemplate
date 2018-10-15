using System;
using System.Threading.Tasks;
using Core.Services.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Helpers
{
    public abstract class BaseListApiController<T, TDomainList, TFk> : BaseApiController<T>
        where T : IListBaseService<TDomainList, TFk>
        where TFk : struct
    {
        protected BaseListApiController(T service, ILogger<BaseApiController<T>> logger) : base(service, logger)
        {
        }

        /// <summary>
        ///Méthode HTTP GET de récupération de l'ensemble des enregistrements d'une entité.
        /// </summary>
        /// <param name="foreignKey">Clé étrangère servant à filtrer si elle est présente</param>
        /// <param name="currentPage">Paramètre de pagination : page courante de la recherche.</param>
        /// <param name="rowCount">Paramètre de pagination : nombre de résultats remontés par recherche.</param>
        /// <param name="sort">Paramètre de tri : champ par lequel les enregistrements sont triés.</param>
        /// <param name="searchPhrase">Paramètre de filtre : chaîne de caractères de filtre de résultat.</param>
        /// <returns></returns>
        [HttpGet("List")]
        public virtual async Task<IActionResult> Get(TFk? foreignKey = null, int? currentPage = null, int? rowCount = null, string sort = null, string searchPhrase = null)
        {
            try
            {
                if (currentPage == null && rowCount == null && sort == null && searchPhrase == null)
                {
                    var result = await this.Service.All(foreignKey);
                    if (result == null || result.Length == 0)
                        return NoContent();

                    return this.Ok(result);
                }
                else
                {
                    var result = await this.Service.List(out var total, foreignKey, currentPage ?? 1, rowCount ?? 0, sort, searchPhrase);
                    if (result == null || result.Length == 0)
                        return NoContent();

                    return this.Ok(new { total, Lists = result});
                }

            }
            catch (Exception ex)
            {
                this.Logger.LogError("Une erreur interne est survenue lors de la récupération des données", ex);
                return this.StatusCode(500);
            }
        }

    }
}