using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DataNoSql.Utilities
{
    public static class ListUtils
    {
        public static IEnumerable<T> ToPageList<T>(
            this IEnumerable<T> query,
            [Range(minimum: 1, maximum: uint.MaxValue)] uint pageSize = 25,
            [Range(minimum: 1, maximum: uint.MaxValue)] uint page = 1)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 25;
            return query
                .Skip((int)((page - 1) * pageSize))
                .Take((int)pageSize)
                .ToList();
        }
        public static IEnumerable<T> ToPageList<T>(
            this IQueryable<T> query,
            [Range(minimum: 1, maximum: uint.MaxValue)] uint pageSize = 25,
            [Range(minimum: 1, maximum: uint.MaxValue)] uint page = 1)
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 25;
            return query
                .Skip((int)((page - 1) * pageSize))
                .Take((int)pageSize)
                .ToList();
        }
    }
}
