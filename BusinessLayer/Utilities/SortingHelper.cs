using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Utilities
{
    public static class SortingHelper
    {
        public static IQueryable<T> ApplySorting<T>(IQueryable<T> query, string sortField, string sortOrder)
        {
            if (string.IsNullOrEmpty(sortField))
                return query;
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, sortField);
            var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

            if (string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase))
                return query.OrderByDescending(lambda);
            else
                return query.OrderBy(lambda);
        }
    }
}
