using System;

namespace Core.Domains.Base
{
    /// <summary>
    /// Interface de tout objet de domaine
    /// </summary>
    public interface IDomainObject : ICloneable
    {
        bool IsEmpty();
    }
}
