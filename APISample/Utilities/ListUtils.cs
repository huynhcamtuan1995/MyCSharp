using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISample.Utilities
{
    public static class ListUtils
    {
        public static IEnumerable<T> ToPageList<T>(this IEnumerable<T> query, int pageSize = 25, int page = 0)
        {
            return query.Skip((int)((page - 1) * pageSize)).Take((int)pageSize).ToList();
        }
        public static IQueryable<T> ToPageList<T>(this IQueryable<T> query, int pageSize = 25, int page = 0)
        {
            return query.Skip((int)((page - 1) * pageSize)).Take((int)pageSize);
        }
    }
}
