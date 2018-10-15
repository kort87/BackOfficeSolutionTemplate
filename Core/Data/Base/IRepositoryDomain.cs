using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Domains.Base;

namespace Core.Data.Base
{
    /// <summary>
    /// Interface mettant à disposition des méthodes utilisant le repository pour donner des
    /// actions sur les objets du domaines directement
    /// </summary>
    /// <typeparam name="TDomain">Type d'interface supporté pour le DTO unitaire</typeparam>
    /// <typeparam name="TDomainList">Type d'interface supporté pour le DTO List</typeparam>
    /// <typeparam name="TPk">Type de la clé primaire de la table portée par le repository</typeparam>
    /// <typeparam name="TFk">Type de ma clé étrangere de filtrage (mettre object? si non utilisé) </typeparam>
    public interface IRepositoryDomain<TDomain, TDomainList, TPk, TFk> : IDisposable
        where TDomain : IDomainObject
        where TDomainList : IDomainObject
        where TFk : struct
    {
        /// <summary>
        /// Renvoie un objet DTO mappé à partir d'un poco et selectionné grace à sa clef primaire
        /// </summary>
        /// <param name="id">Valeur de la clé primaire de l'objet</param>
        /// <returns>DTO mappé</returns>
        TDomain GetDomainObject(TPk id);
        /// <summary>
        /// Renvoie un objet DTO mappé à partir d'un poco et selectionné grace à sa clef primaire
        ///  > Asynchrone
        /// </summary>
        /// <param name="id">Valeur de la clé primaire de l'objet</param>
        /// <returns>DTO mappé</returns>
        Task<TDomain> GetDomainObjectAsync(TPk id);
        /// <summary>
        /// Insert un DTO dans la base de données
        /// </summary>
        /// <param name="domainObject">Le DTO à inserer</param>
        /// <param name="withTransaction">Indique si on doit utiliser une transaction</param>
        /// <returns>La valeur de la clé primaire du DTO créé ou null si ca n'a pas fonctionné</returns>
        TPk InsertDomainObject(TDomain domainObject, bool withTransaction = true);
        /// <summary>
        /// Update un DTO dans la base de données
        /// </summary>
        /// <param name="domainObject">Le DTO à updater</param>
        /// <param name="withTransaction">Indique si on doit utiliser une transaction</param>
        /// <returns>True si ok  False si ca n'a pas fonctionné</returns>
        bool UpdateDomainObject(TDomain domainObject, bool withTransaction = true);
        /// <summary>
        /// Insert ou Update un DTO dans la base de données
        /// </summary>
        /// <param name="domainObject">Le DTO à inserer ou updater</param>
        /// <param name="withTransaction">Indique si on doit utiliser une transaction</param>
        /// <returns>True si ok  False si ca n'a pas fonctionné</returns>
        bool InsertOrUpdateDomainObject(TDomain domainObject, bool withTransaction = true);
        /// <summary>
        /// Supprime un enregistrement dans la base de données
        /// </summary>
        /// <param name="id">Valeur de la clé primaire de l'enr à supprimer</param>
        /// <param name="withTransaction">Indique si on doit utiliser une transaction</param>
        /// <returns>True si ok  False si ca n'a pas fonctionné</returns>
        bool Delete(TPk id, bool withTransaction = true);
        /// <summary>
        /// Methode de mapping pour les listes de poco vers une liste de DTO
        /// </summary>
        /// <param name="predicateFilter">Predicat de filtrage</param>
        /// <param name="sort">Chaine contenant les champs pour trier la liste</param>
        /// <param name="searchPhrase">Chaine contenant un filtre à appliquer</param>
        /// <param name="rowCount">Donne le nombre maximum de ligne à lire</param>
        /// <returns>Un tableau de DTOs</returns>
        TDomainList[] ListMap(object predicateFilter, string sort = null, string searchPhrase = null, int rowCount = 0);
        /// <summary>
        /// Methode de mapping pour les listes de poco vers une liste de DTO
        ///   > Asynchrone
        /// </summary>
        /// <param name="predicateFilter">Predicat de filtrage</param>
        /// <param name="sort">Chaine contenant les champs pour trier la liste</param>
        /// <param name="searchPhrase">Chaine contenant un filtre à appliquer</param>
        /// <param name="rowCount">Donne le nombre maximum de ligne à lire</param>
        /// <returns>Un tableau de DTOs</returns>
        Task<TDomainList[]> ListMapAsync(object predicateFilter, string sort = null, string searchPhrase = null, int rowCount = 0);
        /// <summary>
        /// Methode de mapping pour les listes de poco vers une liste de DTO
        /// spécialisé dans les tableaux (IHM) paginés
        /// </summary>
        /// <param name="total">Donne en sortie le nombre total de ligne de la table(selon les critères de filtrage)</param>
        /// <param name="predicateFilter">Predicat de filtrage</param>
        /// <param name="foreignKey">Valeur de filtrage sur une foreign (null=pas de filtre)</param>
        /// <param name="sort">Chaine contenant les champs pour trier la liste</param>
        /// <param name="searchPhrase">Chaine contenant un filtre à appliquer</param>
        /// <param name="currentPage">Index de la page à lire</param>
        /// <param name="rowCount">Donne le nombre maximum de ligne à lire</param>
        /// <returns></returns>
        IEnumerable<TDomainList> PaginateMap(out int total,  object predicateFilter, TFk? foreignKey , string sort, string searchPhrase, int currentPage, int rowCount);
        /// <summary>
        /// Methode de mapping pour les listes de poco vers une liste de DTO
        /// spécialisé dans les tableaux (IHM) paginés
        /// </summary>
        /// <param name="total">Donne en sortie le nombre total de ligne de la table(selon les critères de filtrage)</param>
        /// <param name="predicateFilter">Predicat de filtrage</param>
        /// <param name="foreignKey">Valeur de filtrage sur une foreign (null=pas de filtre)</param>
        /// <param name="sort">Chaine contenant les champs pour trier la liste</param>
        /// <param name="searchPhrase">Chaine contenant un filtre à appliquer</param>
        /// <param name="currentPage">Index de la page à lire</param>
        /// <param name="rowCount">Donne le nombre maximum de ligne à lire</param>
        /// <returns></returns>
        Task<TDomainList[]> PaginateMapAsync(out int total, object predicateFilter, TFk? foreignKey, string sort, string searchPhrase, int currentPage, int rowCount);
    }
}