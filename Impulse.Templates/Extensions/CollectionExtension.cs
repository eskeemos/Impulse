using System.Collections.Generic;
using System.Linq;

namespace Impulse.Shared.Extensions
{
    public static class CollectionExtension
    {
        public static bool AnyAndNotNull<T>(this IEnumerable<T> source)
            => source != null && source.Any();
    }
}
