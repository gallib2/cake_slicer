using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace System.Collections.Generic
{
    public static class CollectionExtenstions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> collection) => collection.Count() == 0;
    }
}

