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

        /// <summary>
        /// Removes a range of items specified in <paramref name="remove"/> from <paramref name="collection"/>, and then calls <see cref="ICollection{T}.Clear"/> on <paramref name="remove"/>
        /// </summary>
        public static void RemoveClear<T>(this ICollection<T> collection, ICollection<T> remove)
        {
            collection.RemoveRange(remove);
            remove.Clear();
        }
    }
}
