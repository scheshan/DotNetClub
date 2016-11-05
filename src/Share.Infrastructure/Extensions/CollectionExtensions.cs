using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Share.Infrastructure.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsEmptyCollection<T>(this IEnumerable<T> list)
        {
            if (list == null)
            {
                return true;
            }
            return !list.Any();
        }
    }
}
