using System.Threading.Tasks;

namespace Core.Services.Base
{
    /// <summary>
    /// Interface décrivant les services de type Create Read Update Delete
    /// </summary>
    /// <typeparam name="TDomainType">Le type d'objet Domain</typeparam>
    /// <typeparam name="TDomainList">Le type d'objet Domain</typeparam>
    /// <typeparam name="TPk">le type de Primary Key de l'objet</typeparam>
    /// <typeparam name="TFk">Clé etrangere de filtrage</typeparam>
    public interface ICRUDBaseService<TDomainType, TDomainList, TPk, TFk> :
        IListBaseService<TDomainList, TFk>
        where TFk : struct
    {
        /// <summary>
        /// Lire un Domain Object depuis une source de données (repository)
        /// </summary>
        /// <param name="id">Valeur de primary key</param>
        /// <returns></returns>
        Task<TDomainType> Load(TPk id);
        /// <summary>
        /// Creer dans la source de données un Domaine Object
        /// </summary>
        /// <param name="entity">L'objet à créer</param>
        /// <returns>La valeur de la nouvelle clé primaire de l'objet</returns>
        TPk Create(TDomainType entity);
        /// <summary>
        /// Mettre à jour dans la source de données un Domain Objet
        /// </summary>
        /// <param name="entity">L'objet à mettre à jour</param>
        /// <returns>Vrai s'il a été mis à jours</returns>
        bool Update(TDomainType entity);
        /// <summary>
        /// Supprime un objet dans la source de données
        /// </summary>
        /// <param name="id">La valeur de clé primaire de l'objet à supprimer</param>
        /// <returns>Vrai si l'objet a été supprimer avec succés</returns>
        bool Delete(TPk id);
    }
}
