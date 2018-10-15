using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Domains.Base;

namespace Core.Data.Base
{
    /// <summary>
    /// Interface implementant la pattern Repository et faisant le lien entre POCO et DTO
    /// </summary>
    /// <typeparam name="TPoco">Type d'interface du poco</typeparam>
    /// <typeparam name="TDomain">Type d'interface du DTO</typeparam>
    /// <typeparam name="TDomainList">Type d'interface du DTO List</typeparam>
    /// <typeparam name="TPk">Type de clé primaire</typeparam>
    /// <typeparam name="TFk">Type de ma clé étrangere de filtrage (par convention mettre bool si non utilisé) </typeparam>
    public interface IRepository<TPoco, TDomain, TDomainList, TPk, TFk> : IRepositoryDomain<TDomain, TDomainList, TPk, TFk>
        where TPoco : class, IEntity
        where TDomain : IDomainObject
        where TDomainList : IDomainObject
        where TFk : struct
    {
        /// <summary>
        /// Expression qui permet d'acceder à un enregistrement par sa/ses clé primaire
        /// </summary>
        /// <param name="pkKey">La valeur de la clé primaire</param>
        /// <returns>Predicat pret à l'emploi pour un where</returns>
        Expression<Func<TPoco, bool>> GetByKeyPredicate(TPk pkKey);
        /// <summary>
        /// Expression qui permet d'acceder à un enregistrement par sa/ses clé étrangère
        /// </summary>
        /// <param name="fkKey">La valeur de la clé etrangère</param>
        /// <returns>Predicat pret à l'emploi pour un where</returns>
        Expression<Func<TPoco, bool>> FilterByForeignKey(TFk? fkKey);
        /// <summary>
        /// Expression qui permet de qualifier le prédicat "sort" d"une query
        /// </summary>
        /// <param name="sortField">le nom du champ à trier</param>
        /// <returns>Prédicat de tri en fonction du champ Sortfield</returns>
        Expression<Func<TPoco, object>> SortPredicate(string sortField);
        /// <summary>
        /// Compte le nombre d'enregristement en fonction du prédicat (where)
        /// </summary>
        /// <param name="predicate">Prédicat de filtre</param>
        /// <returns>le nombre d'enregistrement filtrer par le predicat</returns>
        long Count(Expression<Func<TPoco, bool>> predicate);
        /// <summary>
        /// Compte le nombre d'enregristement en fonction du prédicat (where) en asynchrone
        /// </summary>
        /// <param name="predicate">Prédicat de filtre</param>
        /// <returns>le nombre d'enregistrement filtrer par le predicat</returns>
        Task<long> CountAsync(Expression<Func<TPoco, bool>> predicate);
        /// <summary>
        /// Expression qui permet de qualifier le predicate de recherche "texte libre"
        /// Contient en général les champs texte sur lesquelles s'applique cette recherche
        /// </summary>
        /// <param name="searchPhrase">le texte recherché</param>
        /// <returns>Prédicat de filtre prêt à l'emploi</returns>
        Expression<Func<TPoco, bool>> SearchQuery(string searchPhrase);

        /// <summary>
        /// Renvoi une query paginée, triée et filtrée selon les différents paramètres
        /// Idéal pour les listes
        /// </summary>
        /// <param name="total">permet le retour du nombre total de lignes</param>
        /// <param name="filter">expression de filtrage à appliquer</param>
        /// <param name="foreignKey"></param>
        /// <param name="sort">champ (et ordre) sur lequel la query doit être trier</param>
        /// <param name="searchPhrase">texte libre de recherche</param>
        /// <param name="index">Page de départ de la query</param>
        /// <param name="size">Taille de la page (10 par défaut)</param>
        /// <returns>Un query pour liste paginée prête à l'emploi</returns>
        IQueryable<TPoco> Paginate(out int total, Expression<Func<TPoco, bool>> filter, TFk? foreignKey, string sort, string searchPhrase, int index, int size);

        /// <summary>
        /// Renvoi une query filtrée asynchrone
        /// </summary>
        /// <param name="predicate">expression de filtrage à appliquer</param>
        /// <param name="searchPhrase"></param>
        /// <param name="max">nombre d'enregisterement max désiré (0 par défaut : tous les enregistrements)</param>
        /// <param name="foreignKey"></param>
        /// <param name="sort"></param>
        /// <returns>Un query pour liste prête à l'emploi</returns>
        Task<TPoco[]> ListAsync(Expression<Func<TPoco, bool>> predicate,TFk? foreignKey = null, string sort = null, string searchPhrase = null, int max = 0);

        /// <summary>
        /// Renvoi une query triée et filtrée selon les différents paramètres
        /// Idéal pour les traitements en masses
        /// </summary>
        /// <param name="filter">expression de filtrage à appliquer</param>
        /// <param name="foreignKey"></param>
        /// <param name="sort">champ (et ordre) sur lequel la query doit être trier</param>
        /// <param name="searchPhrase">texte libre de recherche</param>
        /// <param name="size">nombre d'enregisterement max désiré (0 par défaut : tous les enregistrements)</param>
        /// <returns>Un query pour liste prête à l'emploi</returns>
        IQueryable<TPoco> List(Expression<Func<TPoco, bool>> filter, TFk? foreignKey, string sort, string searchPhrase, int size);
        /// <summary>
        /// Obtient un enregistrement (poco) à partir de sa clé unique
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un poco objet</returns>
        TPoco Get(TPk id);
        /// <summary>
        /// Obtient un enregistrement (poco) à partir de sa clé unique asynchrone
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un poco objet</returns>
        Task<TPoco> GetAsync(TPk id);
        /// <summary>
        /// Fabrique une query a partir de la table et lui applique un filtre 
        /// </summary>
        /// <param name="predicate">Le prédicat du filtre</param>
        /// <returns>Une query filtré</returns>
        IQueryable<TPoco> Get(Expression<Func<TPoco, bool>> predicate);
        /// <summary>
        /// Insert un objet Poco dans la base
        /// </summary>
        /// <param name="entity">le Poco objet à inserer</param>
        /// <param name="withTransaction">Utilise un système de transaction (oui par défaut)</param>
        /// <returns>Retourne la clé primaire du nouvel enregistrement</returns>
        TPk Insert(TPoco entity, bool withTransaction = true);
        /// <summary>
        /// Insert un objet Poco ou le met à jour s'il existe déjà
        /// </summary>
        /// <param name="entity">Objet poco à inserer/updater</param>
        /// <param name="withTransaction">Utilise un système de transaction (oui par défaut)</param>
        /// <returns>Vrai si l'opération s'est bien passé</returns>
        bool InsertOrUpdate(TPoco entity, bool withTransaction = true);
        /// <summary>
        /// Met à jour un objet Poco
        /// </summary>
        /// <param name="entity">Objet poco à inserer/updater</param>
        /// <param name="withTransaction">Utilise un système de transaction (oui par défaut)</param>
        /// <returns>Vrai si l'opération s'est bien passé</returns>
        bool Update(TPoco entity, bool withTransaction = true);
        /// <summary>
        /// Méthode de mappage pour les listes (POCO -> Domain)
        /// Par défaut, elle utilise le mappage standard
        /// A implementer si on veut des listes avec moins d'information
        /// </summary>
        /// <param name="pocoObject">L'objet à mapper</param>
        /// <returns>le domain (list) objet mappé</returns>
        TDomainList MapList(TPoco pocoObject);
        /// <summary>
        /// Méthode de mappage  (POCO -> Domain)
        /// </summary>
        /// <param name="pocoObject">L'objet à mapper</param>
        /// <returns>le domain (list) objet mappé</returns>
        TDomain Map(TPoco pocoObject);
        /// <summary>
        /// Méthode de convertion (Domain -> Poco)
        /// Utiliser surtout pour les updates
        /// </summary>
        /// <param name="domainObject">Le domain objet à convertir</param>
        /// <returns>L'objet Poco convertie</returns>
        TPoco Convert(TDomain domainObject);
    }
}
