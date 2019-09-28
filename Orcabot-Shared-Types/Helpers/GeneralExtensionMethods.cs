using System;
using System.Collections.Generic;
using System.Text;

namespace Orcabot.Helpers
{
    public static  class GeneralExtensionMethods
    {
        /// <summary>
        /// Removes a range of items from a collection
        /// </summary>
        /// <param name="collection">Collection to remove items from</param>
        /// <param name="remove">Enumerable of items to remove</param>
        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> remove)
        {
            foreach (T item in remove)
            {
                collection.Remove(item);
            }
        }
    }
}
