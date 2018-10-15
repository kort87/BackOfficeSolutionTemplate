using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core;
using Core.Data.Base;
using Core.Domains.Base;
using LinqToDB;
using Microsoft.Extensions.Logging;

namespace Data.Helpers
{
    public abstract partial class BaseRepository<TPoco, TDomain, TDomainList, TPk, TFk> : IRepository<TPoco, TDomain, TDomainList, TPk, TFk>
        where TPoco : class, IEntity
        where TDomain : IDomainObject
        where TDomainList : IDomainObject
        where TFk : struct
    {
        // ReSharper disable InconsistentNaming
        public const string SORTFIELD_ID = "Id";
        public const string SORTFIELD_NOM = "Nom";
        public const string SORTFIELD_DESIGNATION = "Designation";
        public const string SORTFIELD_CODE = "Code";
        // ReSharper restore InconsistentNaming


        private LinqToDB.Data.DataConnection _db;

        /// <summary>
        /// Connection à la base de données
        /// </summary>
        protected LinqToDB.Data.DataConnection DB => this._db ?? (this._db = new LinqToDB.Data.DataConnection());

        private IQueryable<TPoco> _table;

        /// <summary>
        /// Query sur la table
        /// </summary>
        protected IQueryable<TPoco> Table
        {
            get => this._table ?? (this._table = this.GetQueryable());
            set => this._table = value;
        }

        protected virtual IQueryable<TPoco> GetQueryable()
        {
            return this.DB.GetTable<TPoco>();
        }

        private ITable<TPoco> _directTable;
        /// <summary>
        /// Référence directe sur la table, utile pour les définitions de jointures (et autres LoadWith).
        /// </summary>
        protected ITable<TPoco> DirectTable
        {
            get => this._directTable ?? (this._directTable = this.DB.GetTable<TPoco>());
            set => this._directTable = value;
        }
        protected IDomainObjectFactory<TDomain> DomainObjectFactory;
        protected IDomainObjectFactory<TDomainList> DomainObjectListFactory;

        //protected KeiretsuDevDB KeiretsuDevDBAccessor => (KeiretsuDevDB)this.DB;

        protected ILogger Logger;

        protected BaseRepository(LinqToDB.Data.DataConnection db, ILogger logger, IDomainObjectFactory<TDomain> factory, IDomainObjectFactory<TDomainList> listFactory = null)
        {
            this._db = db;
            this.DomainObjectFactory = factory;
            this.DomainObjectListFactory = listFactory;
            this.Logger = logger;
        }

        /// <summary>
        /// Expression qui permet d'acceder à un enregistrement par sa/ses clé primaire
        /// </summary>
        /// <param name="pkKey">La valeur de la clé primaire</param>
        /// <returns>Expression prete à l'emploi pour un where</returns>
        public abstract Expression<Func<TPoco, bool>> GetByKeyPredicate(TPk pkKey);

        /// <summary>
        /// Expression qui permet de filtrer par sa/ses clé étrangère
        /// </summary>
        /// <param name="fkKey">La valeur de la clé etrangère</param>
        /// <returns>Predicat pret à l'emploi pour un where</returns>
        public virtual Expression<Func<TPoco, bool>> FilterByForeignKey(TFk? fkKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Expression qui permet de qualifier le prédicat "sort" d"une query
        /// </summary>
        /// <param name="sortField">le nom du champ à trier</param>
        /// <returns>Prédicat de tri en fonction du champ Sortfield</returns>
        public abstract Expression<Func<TPoco, object>> SortPredicate(string sortField);

        /// <summary>
        /// Compte le nombre d'enregristement en fonction du prédicat (where)
        /// </summary>
        /// <param name="predicate">Prédicat de filtre</param>
        /// <returns>le nombre d'enregistrement filtrer par le predicat</returns>
        public long Count(Expression<Func<TPoco, bool>> predicate)
        {
            return this.Table.LongCount(predicate);
        }

        /// <summary>
        /// Compte le nombre d'enregristement en fonction du prédicat (where) en asynchrone
        /// </summary>
        /// <param name="predicate">Prédicat de filtre</param>
        /// <returns>le nombre d'enregistrement filtrer par le predicat</returns>
        public Task<long> CountAsync(Expression<Func<TPoco, bool>> predicate)
        {
            return this.Table.LongCountAsync(predicate);
        }

        /// <summary>
        /// Expression qui permet de qualifier le predicate de recherche "texte libre"
        /// Contient en général les champs texte sur lesquelles s'applique cette recherche
        /// </summary>
        /// <param name="searchPhrase">le texte recherché</param>
        /// <returns>Prédicat de filtre prêt à l'emploi</returns>
        public virtual Expression<Func<TPoco, bool>> SearchQuery(string searchPhrase)
        {
            return null;
        }

        /// <summary>
        /// Renvoi une query paginée, triée et filtrée selon les différents paramètres
        /// Idéal pour les listes
        /// </summary>
        /// <param name="total">permet le retour du nombre total de lignes</param>
        /// <param name="foreignKey">une clé étrangère pour les filtres</param>
        /// <param name="filter">expression de filtrage à appliquer</param>
        /// <param name="sort">champ (et ordre) sur lequel la query doit être trier</param>
        /// <param name="searchPhrase">texte libre de recherche</param>
        /// <param name="index">Page de départ de la query</param>
        /// <param name="size">Taille de la page (10 par défaut)</param>
        /// <returns>Un query pour liste paginée prête à l'emploi</returns>
        public virtual IQueryable<TPoco> Paginate(out int total, Expression<Func<TPoco, bool>> filter = null, TFk? foreignKey = null,
            string sort = null, string searchPhrase = null, int index = 1, int size = 10)
        {

            var query = this.RootList(filter, foreignKey, sort, searchPhrase);

            total = query.Count();

            int skipCount = (index - 1) * size;
            query = skipCount == 0 ? query : query.Skip(skipCount);
            query = size > 0 ? query.Take(size) : query;

            return query;
        }

        public virtual Task<TPoco[]> ListAsync(Expression<Func<TPoco, bool>> predicate,
            TFk? foreignKey=null, string sort = null, string searchPhrase = null, int max=0)
        {
            var query = this.List(predicate, foreignKey,sort,searchPhrase, max);
            return  query.ToArrayAsync();
        }

        public virtual IQueryable<TPoco> List(Expression<Func<TPoco, bool>> filter = null, TFk? foreignKey = null,
            string sort = null, string searchPhrase = null, int size = 0)
        {
            var query = this.RootList(filter, foreignKey, sort, searchPhrase);

            query = size > 0 ? query.Take(size) : query;

            return query;
        }

        private IQueryable<TPoco> RootList(Expression<Func<TPoco, bool>> filter = null, TFk? foreignKey = null,
            string sort = null, string searchPhrase = null)
        {
            if (!foreignKey.IsNull())
            {
                // Un foreignKey de filtrage a été settér
                // donc au filtre "standard", on ajoute le filtre de foreign key 
                // fournie par la méthode FilterByForeignKey
                // S'il y a une valeur mais pas d'implementation dans FilterByForeignKey : Ca plante
                filter = filter == null ?
                    this.FilterByForeignKey(foreignKey) :
                    filter.And(this.FilterByForeignKey(foreignKey));
            }

            var query = filter != null ? this.GetRootQueryList().Where(filter) : this.GetRootQueryList();

            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                var expression = this.SearchQuery(searchPhrase);
                if (expression != null)
                    query = query.Where(expression);
            }

            if (sort == null) return query;

            var sortParam = sort.Split('.');
            var order = sortParam.Length == 1 || sortParam[1] == "asc" ? SortOrder.Ascending : SortOrder.Descending;
            query = query.ApplySorting(this.SortPredicate(sortParam[0]), order);

            return query;
        }

        /// <summary>
        /// Permet de fabriquer une liste de base différente de la stucture de la base
        /// Plus courte ou avec des jointure par exemple
        /// Par défaut, c'est la table
        /// </summary>
        /// <returns>Une query</returns>
        protected virtual IQueryable<TPoco> GetRootQueryList()
        {
            return this.Table;
        }

        /// <summary>
        /// Fabrique une query a partir de la table et lui applique un filtre 
        /// </summary>
        /// <param name="predicate">Le prédicat du filtre</param>
        /// <returns>Une query filtré</returns>
        public virtual IQueryable<TPoco> Get(Expression<Func<TPoco, bool>> predicate)
        {
            return this.Table.Where(predicate);
        }

        public void Dispose()
        {

        }
    }
}
