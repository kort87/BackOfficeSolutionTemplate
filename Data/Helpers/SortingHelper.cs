using System;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Helpers
{
    public static class SortingHelper
    {
        public static IQueryable<T> ApplySorting<T, TU>(this IQueryable<T> query, Expression<Func<T, TU>> predicate, SortOrder order)
        {
            // Créé une query de type trié
            var ordered = query as IOrderedQueryable<T>;
            // la query d'origine est elle déjà trié
            var isordered = query.Expression.Type == typeof(IOrderedQueryable<T>);

            if (order == SortOrder.Ascending)
            {
                // si elle est dejà trié
                if (isordered && ordered != null)
                    // on ajoute ce tri supplémentaire
                    return ordered.ThenBy(predicate);
                // sinon c'est un tri simple
                return query.OrderBy(predicate);
            }

            if (isordered && ordered != null)
                return ordered.ThenByDescending(predicate);
            return query.OrderByDescending(predicate);
        }

    }
}
