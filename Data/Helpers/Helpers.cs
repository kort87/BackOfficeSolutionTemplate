using System.ComponentModel;

namespace Data.Helpers
{
    public static class Helpers
    {
        /// <summary>
        /// Essai de parse depuis un chaine vers un type générique
        /// </summary>
        /// <typeparam name="T">Type cible</typeparam>
        /// <param name="s">Chaine représentant la valeur à convertir</param>
        /// <param name="value">La valeur convertie dans le bon type</param>
        /// <returns>Vrai si on a réussi à convertir, faux : pas de conversion et un valeur "par défaut"</returns>
        public static bool TryParse<T>(string s, out T value)
        {
            // Cherche la définition du type ciblé
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            try
            {
                // tente une conversion
                value = (T)converter.ConvertFromString(s);
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        // Exemple de fabrication d'un clause SQL supplémentaire
        //[Sql.Expression("{0} BETWEEN {1} AND {2}", PreferServerSide = true)]
        //public static bool Between<T>(this T x, T low, T high) where T : IComparable<T>
        //{
        //    // x >= low && x <= high
        //    return x.CompareTo(low) >= 0 && x.CompareTo(high) <= 0;
        //}
    }
}
