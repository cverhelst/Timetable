using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Model
{
    static class Extensions
    {
        // Utilities
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static int GetAltHashCode<T>(this IList<T> listToHash) where T : IComparable
        {
            return listToHash.OrderBy(x => x).Aggregate(0, (x, y) => x.GetHashCode() ^ y.GetHashCode());
        }

        public static int GetAltHashCode<T>(this ISet<T> stackToHash) where T : IComparable
        {
            return stackToHash.OrderBy(x => x).Aggregate(0, (x, y) => x.GetHashCode() ^ y.GetHashCode());
        }

        public static ISet<T> Clone<T>(this ISet<T> setToClone) where T : ICloneable
        {
            return new SortedSet<T>(setToClone.Select(item => (T)item.Clone()));
        }

        public static DateTime DateTimeCreator(int day, int hour, int minute)
        {
            DateTime now = DateTime.Now;
            DateTime to = new DateTime(now.Year, now.Month, (day % 365) + 1, hour, minute, 0);
            return to;
        }

        // TimeUnit specific

        public static SortedSet<T> MergeUnits<T>(this SortedSet<T> setToMerge) where T : TimeUnit
        {
            SortedSet<T> set = (SortedSet<T>) setToMerge.Clone();
            int size = setToMerge.Count;
            int newSize = size;

            while (newSize != size)
            {
                int index = 0;
                bool toNext = true;

                while (index < set.Count)
                {
                    T unit = set.ElementAt(index);
                    foreach (T toMerge in set)
                    {
                        if (toMerge.IsConsecutiveWith(unit))
                        {
                            unit.Merge(toMerge);
                            set.Remove(toMerge);
                            toNext = false;
                            break;
                        }
                    }

                    if (toNext)
                    {
                        index++;
                    }
                    else
                    {
                        index = 0;
                    }
                }
            }
            return set;
        }
    }
}
