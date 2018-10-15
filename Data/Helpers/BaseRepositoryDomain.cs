using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Data.Base;
using Core.Domains.Base;
using LinqToDB;

namespace Data.Helpers
{
    public abstract partial class BaseRepository<TPoco, TDomain, TDomainList, TPk, TFk> where TPoco : class, IEntity
        where TDomain : IDomainObject
        where TDomainList : IDomainObject
        where TFk : struct

    {
        /// <summary>
        /// Renvoie un objet DTO mappé à partir d'un poco et selectionné grace à sa clef primaire
        /// </summary>
        /// <param name="id">Valeur de la clé primaire de l'objet</param>
        /// <returns>DTO mappé</returns>
        public virtual TDomain GetDomainObject(TPk id)
        {
            return this.Map(this.Get(id));
        }

        /// <summary>
        /// Renvoie un objet DTO mappé à partir d'un poco et selectionné grace à sa clef primaire
        ///  > Asynchrone
        /// </summary>
        /// <param name="id">Valeur de la clé primaire de l'objet</param>
        /// <returns>DTO mappé</returns>
        public virtual async Task<TDomain> GetDomainObjectAsync(TPk id)
        {
            return this.Map(await this.GetAsync(id));
        }

        #region Listes
        #region Listes paginées
        public virtual Task<TDomainList[]> PaginateMapAsync(out int total, object predicateFilter = null,
            TFk? foreignKey = null, string sort = null, string searchPhrase = null, int currentPage = 1,
            int rowCount = 10)
        {
            var query = this.InternalPaginateMap(out total, predicateFilter, foreignKey, sort, searchPhrase,
                currentPage, rowCount);

            return query.ToArrayAsync();
        }

        public virtual IEnumerable<TDomainList> PaginateMap(out int total, object predicateFilter = null,
            TFk? foreignKey = null, string sort = null, string searchPhrase = null, int currentPage = 1,
            int rowCount = 10)
        {
            var query = this.InternalPaginateMap(out total, predicateFilter, foreignKey, sort, searchPhrase,
                currentPage, rowCount);

            return query.ToArray();
        }

        protected virtual IQueryable<TDomainList> InternalPaginateMap(out int total, object predicateFilter = null,
            TFk? foreignKey = null, string sort = null, string searchPhrase = null, int currentPage = 1,
            int rowCount = 10)
        {
            var query = this.Paginate(out total, (Expression<Func<TPoco, bool>>)predicateFilter, foreignKey, sort, searchPhrase,
                currentPage, rowCount);
            return from p in query
                   select this.MapList(p);
        }
        #endregion

        #region Listes simples
        public virtual TDomainList[] ListMap(object predicateFilter = null, string sort = null,
            string searchPhrase = null, int rowCount = 0)
        {
            var query = this.InternalListMap(predicateFilter, sort, searchPhrase, rowCount);
            return query.ToArray();
        }

        public virtual Task<TDomainList[]> ListMapAsync(object predicateFilter = null, string sort = null,
            string searchPhrase = null, int rowCount = 0)
        {
            var query = this.InternalListMap(predicateFilter, sort, searchPhrase, rowCount);
            return query.ToArrayAsync();
        }

        protected virtual IQueryable<TDomainList> InternalListMap(object predicateFilter = null, string sort = null,
            string searchPhrase = null, int rowCount = 0)
        {
            var query = this.List((Expression<Func<TPoco, bool>>)predicateFilter, null, sort, searchPhrase, rowCount);
            return from p in query
                   select this.MapList(p);
        }
        #endregion
        #endregion

        #region CRUD

        /// <summary>
        /// Insert un DTO dans la base de données
        /// </summary>
        /// <param name="domainObject">Le DTO à inserer</param>
        /// <param name="withTransaction">Indique si on doit utiliser une transaction</param>
        /// <returns>La valeur de la clé primaire du DTO créé ou null si ca n'a pas fonctionné</returns>
        public virtual TPk InsertDomainObject(TDomain domainObject, bool withTransaction = true)
        {
            return this.Insert(this.Convert(domainObject), withTransaction);
        }

        /// <summary>
        /// Update un DTO dans la base de données.
        /// </summary>
        /// <param name="domainObject">Le DTO à updater</param>
        /// <param name="withTransaction">Indique si on doit utiliser une transaction</param>
        /// <returns>True si ok  False si ca n'a pas fonctionné</returns>
        public virtual bool UpdateDomainObject(TDomain domainObject, bool withTransaction = true)
        {
            return this.Update(this.Convert(domainObject), withTransaction);
        }

        /// <summary>
        /// Insert ou Update un DTO dans la base de données
        /// </summary>
        /// <param name="domainObject">Le DTO à inserer ou updater</param>
        /// <param name="withTransaction">Indique si on doit utiliser une transaction</param>
        /// <returns>True si ok  False si ca n'a pas fonctionné</returns>
        public virtual bool InsertOrUpdateDomainObject(TDomain domainObject, bool withTransaction = true)
        {
            return this.InsertOrUpdate(this.Convert(domainObject), withTransaction);
        }
        #endregion

        /// <summary>
        /// Méthode de mappage  (POCO -> Domain)
        /// </summary>
        /// <param name="pocoObject">L'objet à mapper</param>
        /// <returns>le domain (list) objet mappé</returns>
        public abstract TDomain Map(TPoco pocoObject);

        /// <summary>
        /// Méthode de mappage pour les listes (POCO -> Domain)
        /// Par défaut, elle utilise le mappage standard
        /// A implementer si on veut des listes avec moins d'information
        /// </summary>
        /// <param name="pocoObject">L'objet à mapper</param>
        /// <returns>le domain (list) objet mappé</returns>
        public abstract TDomainList MapList(TPoco pocoObject);

        /// <summary>
        /// Méthode de convertion (Domain -> Poco)
        /// Utiliser surtout pour les updates
        /// </summary>
        /// <param name="domainObject">Le domain objet à convertir</param>
        /// <returns>L'objet Poco convertie</returns>
        public abstract TPoco Convert(TDomain domainObject);
    }
}
