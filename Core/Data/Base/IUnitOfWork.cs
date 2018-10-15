using System;

namespace Core.Data.Base
{
    /// <summary>
    /// La classe d’unité de travail coordonne le travail de plusieurs référentiels
    /// en créant une classe de contexte de base de données unique partagée par tous.
    /// L'interface contient la liste des repositories gérés par le UnitOfWork.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
    }
}
