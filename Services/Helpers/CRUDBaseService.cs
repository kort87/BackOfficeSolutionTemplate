using System.Threading.Tasks;
using Core.Data.Base;
using Core.Domains.Base;
using Core.Services.Base;
using Microsoft.Extensions.Logging;

namespace Services.Helpers
{
    /// <summary>
    /// Classe de base implémentant les services de type Create Read Update Delete
    /// </summary>
    /// <typeparam name="TDomainList">Le type d'objet Domain pour les listes</typeparam>
    /// <typeparam name="TPk">le type de Primary Key de l'objet</typeparam>
    /// <typeparam name="TDomainType">Le type d'objet Domain</typeparam>
    /// <typeparam name="TRepo">Type de repository à utiliser</typeparam>
    /// <typeparam name="TFk">Clé étrangère de filtrage.</typeparam>
    public abstract class CRUDBaseService<TDomainType, TDomainList, TPk, TFk, TRepo> :
        ListBaseService<TDomainType, TDomainList, TPk, TFk, TRepo>, 
        ICRUDBaseService<TDomainType, TDomainList, TPk, TFk>
        where TRepo : IRepositoryDomain<TDomainType, TDomainList, TPk, TFk>
        where TDomainType : IDomainObject
        where TDomainList : IDomainObject
        where TFk : struct
    {
        protected CRUDBaseService(ILogger<BaseService> logsrv, IUnitOfWork unitOfWork) : base(logsrv, unitOfWork)
        {
        }

        /// <summary>
        /// Lire un Domain Object depuis une source de données (repository)
        /// </summary>
        /// <param name="id">Valeur de primary key</param>
        /// <returns></returns>
        public virtual async Task<TDomainType> Load(TPk id)
        {
            var dto = await this.Repository.GetDomainObjectAsync(id);
            return dto;
        }

        /// <summary>
        /// Creer dans la source de données un Domaine Object
        /// </summary>
        /// <param name="entity">L'objet à créer</param>
        /// <returns>La valeur de la nouvelle clé primaire de l'objet</returns>
        public virtual TPk Create(TDomainType entity)
        {
            var result = this.Repository.InsertDomainObject(entity);
            return result;
        }

        /// <summary>
        /// Mettre à jour dans la source de données un Domain Objet
        /// </summary>
        /// <param name="entity">L'objet à mettre à jour</param>
        /// <returns>Vrai s'il a été mis à jours</returns>
        public virtual bool Update(TDomainType entity)
        {
            return this.Repository.UpdateDomainObject(entity);
        }

        /// <summary>
        /// Supprime un objet dans la source de données
        /// </summary>
        /// <param name="id">La valeur de clé primaire de l'objet à supprimer</param>
        /// <returns>Vrai si l'objet a été supprimer avec succés</returns>
        public virtual bool Delete(TPk id)
        {
            return this.Repository.Delete(id);
        }
    }
}
