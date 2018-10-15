using System.Threading.Tasks;
using Core.Data.Base;
using Core.Domains.Base;
using Core.Services.Base;
using Microsoft.Extensions.Logging;

namespace Services.Helpers
{
    public abstract class ListBaseService<TDomain, TDomainList, TPk, TFk, TRepo> :
        BaseService, IListBaseService<TDomainList, TFk>
        where TRepo : IRepositoryDomain<TDomain, TDomainList, TPk, TFk>
        where TDomain : IDomainObject
        where TDomainList : IDomainObject
        where TFk : struct
    {
        protected TRepo Repository { get; set; }

        public abstract TRepo GetRepository();

        protected ListBaseService(ILogger<BaseService> logsrv, IUnitOfWork unitOfWork) : base(logsrv, unitOfWork)
        {
            // Ici, la classe enfant a la charge de définir l'accès au repository utilisé.
            // ReSharper disable once VirtualMemberCallInConstructor
            this.Repository = this.GetRepository();
        }

        public virtual Task<TDomainList[]> List(out int total, TFk? foreignKey, int currentPage, int rowCount,
            string sort, string searchPhrase)
        {
            return this.Repository.PaginateMapAsync(out total, null, foreignKey, sort, searchPhrase, currentPage, rowCount);
        }

        public virtual Task<TDomainList[]> All(TFk? foreignKey = null, string sort = null, string query = null)
        {
            return this.Repository.ListMapAsync(foreignKey, sort, query);
        }
    }
}
