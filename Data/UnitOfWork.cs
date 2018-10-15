using System;
using Core.Data.Base;
using Microsoft.Extensions.Logging;

namespace Data
{
    /// <summary>
    /// La classe d’unité de travail coordonne le travail de plusieurs référentiels
    /// en créant une classe de contexte de base de données unique partagée par tous.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        #region Database
        // Variable de définition de la base de données générée.
        //private KeiretsuDevDB _db;

        /// <summary>
        /// Connection à la base de données
        /// </summary>
        //protected KeiretsuDevDB DB => this._db ?? (this._db = new KeiretsuDevDB());
        #endregion

        private readonly ILogger _logger;

        #region Factories

        #endregion

        #region Repos
        
        #endregion

        #region Constructor

        public UnitOfWork(ILogger<UnitOfWork> logger)
        {
            this._logger = logger;
           
        }
        #endregion

        public bool Commit()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Libère explicitement les repositories et la base par le UnitOfWork lors de sa propre libération.
        /// </summary>
        public void Dispose()
        {
            // Libération des repositories
           

            // Libération de la base
            //this.DB?.Dispose();
        }
    }
}
