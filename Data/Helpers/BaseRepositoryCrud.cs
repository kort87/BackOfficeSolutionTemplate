using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Core.Data.Base;
using Core.Domains.Base;
using LinqToDB;
using Microsoft.Extensions.Logging;

namespace Data.Helpers
{
    public abstract partial class BaseRepository<TPoco, TDomain, TDomainList, TPk, TFk>
        where TPoco : class, IEntity
        where TDomain : IDomainObject
        where TDomainList : IDomainObject
        where TFk : struct
    { 
        public virtual TPoco Get(TPk id)
        {
            return Get(this.GetByKeyPredicate(id)).SingleOrDefault();
        }

        /// <summary>
        /// Obtient un enregistrement (poco) à partir de sa clé unique asynchrone
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un poco objet</returns>
        public virtual Task<TPoco> GetAsync(TPk id)
        {
            return Get(this.GetByKeyPredicate(id)).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Insert un objet Poco dans la base
        /// </summary>
        /// <param name="entity">le Poco objet à inserer</param>
        /// <param name="withTransaction">Utilise un système de transaction (oui par défaut)</param>
        /// <returns>Retourne la clé primaire du nouvel enregistrement</returns>
        public virtual TPk Insert(TPoco entity, bool withTransaction = true)
        {
            try
            {
                if (withTransaction) this.DB.BeginTransaction(IsolationLevel.ReadUncommitted);

                // RBE 24/08 : InsertWithIdentitity remplacé par Insert,
                // ne permettait pas d'insérer un enregistrement avec une valeur de PK prédéfinie.
                // todo RM 20/09 : à la remarque ci dessus je réponds que InsertOrUpdate a été fait pour cela > a remettre en état
                var pks = this.DB.Insert(entity).ToString();

                if (Helpers.TryParse(pks, out TPk pk))
                {
                    if (withTransaction) this.DB.CommitTransaction();
                    return pk;
                }
                if (withTransaction) this.DB.RollbackTransaction();
                throw new LinqToDBException("Unabled to insert row  >" + typeof(TPoco));
            }
            catch (Exception e)
            {
                if (withTransaction) this.DB.RollbackTransaction();

                this.Logger.LogError(e, "SQL Insert Error");
                throw;
            }
        }

        /// <summary>
        /// Insert un objet Poco ou le met à jour s'il existe déjà
        /// </summary>
        /// <param name="entity">Objet poco à inserer/updater</param>
        /// <param name="withTransaction">Utilise un système de transaction (oui par défaut)</param>
        /// <returns>Vrai si l'opération s'est bien passé</returns>
        public virtual bool InsertOrUpdate(TPoco entity, bool withTransaction = true)
        {
            try
            {
                if (withTransaction) this.DB.BeginTransaction(IsolationLevel.ReadUncommitted);

                var result = this.DB.InsertOrReplace(entity) > 0;

                if (withTransaction) this.DB.CommitTransaction();
                return result;
            }
            catch (Exception e)
            {
                if (withTransaction) this.DB.RollbackTransaction();

                this.Logger.LogError(e, "SQL Update Error");
                throw new LinqToDBException("Unabled to update row  >" + typeof(TPoco));
            }
        }

        /// <summary>
        /// Met à jour un objet Poco
        /// </summary>
        /// <param name="entity">Objet poco à inserer/updater</param>
        /// <param name="withTransaction">Utilise un système de transaction (oui par défaut)</param>
        /// <returns>Vrai si l'opération s'est bien passé</returns>
        public virtual bool Update(TPoco entity, bool withTransaction = true)
        {
            try
            {
                if (withTransaction) this.DB.BeginTransaction(IsolationLevel.ReadUncommitted);

                var result = this.DB.Update(entity) > 0;

                if (withTransaction) this.DB.CommitTransaction();
                return result;
            }
            catch (Exception e)
            {
                if (withTransaction) this.DB.RollbackTransaction();

                this.Logger.LogError(e, "SQL Update Error");
                throw new LinqToDBException("Unabled to update row  >" + typeof(TPoco));
            }
        }


        /// <summary>
        /// Supprime un enregistrement dans la base de données
        /// </summary>
        /// <param name="id">Valeur de la clé primaire de l'enr à supprimer</param>
        /// <param name="withTransaction">Indique si on doit utiliser une transaction</param>
        /// <returns>True si ok  False si ca n'a pas fonctionné</returns>
        public virtual bool Delete(TPk id, bool withTransaction = true)
        {
            try
            {
                if (withTransaction) this.DB.BeginTransaction(IsolationLevel.ReadUncommitted);

                var result = Get(this.GetByKeyPredicate(id)).Delete() > 0;

                if (withTransaction) this.DB.CommitTransaction();
                return result;
            }
            catch (Exception e)
            {
                if (withTransaction) this.DB.RollbackTransaction();

                this.Logger.LogError(e, "SQL Delete Error");
                throw new LinqToDBException("Unabled to delete row  >" + typeof(TPoco));
            }
        }
    }
}
