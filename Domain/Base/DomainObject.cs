using Core.Domains.Base;

namespace Domain.Base
{
    /// <summary>
    /// Classe parente de tout objet de domaine
    /// </summary>
    public class DomainObject : IDomainObject
    {
        /// <summary>
        /// Permet de cloner un objet.
        /// </summary>
        /// <returns>L'objet retourné est à caster pour recuperer son type.</returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        /// <summary>
        /// Définition du caractère vide d'un objet.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            throw new System.NotImplementedException();
        }
    }
}
