using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Quze.Infrastruture.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> list)
        {
            return !list.IsNullOrEmpty();
        }
        public static void AddRange<T>(this ConcurrentBag<T> @this, IEnumerable<T> toAdd)
        {
            foreach (var element in toAdd)
            {
                @this.Add(element);
            }
        }

        

        /// <summary>
        /// Converts List<Concrete> to List<Interface>crete>
        /// </summary>
        /// <typeparam name="T"> concrete type </typeparam>
        /// <typeparam name="I"> Interface type </typeparam>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static IEnumerable<I> ForAll<T, I>(this IEnumerable<T> lst) where T : I
        {
            foreach (I item in lst) yield return item;
        }
    }
}
