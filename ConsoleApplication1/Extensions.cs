using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Model
{
    public static class Extensions
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

        public static void MergeUnits<T>(this ICollection<T> setToMerge) where T : TimeUnit
        {
            bool changed;
            do
            {
                // initialize changed to false
                changed = false;
                foreach (T unit in setToMerge)
                {
                    if (unit.AssignedCourse == null)
                    {
                        foreach (T toMerge in setToMerge)
                        {
                            // find the consecutive timeunit to merge with
                            if (toMerge.AssignedCourse == null &&
                                toMerge.IsConsecutiveWith(unit))
                            {
                                unit.Merge(toMerge);
                                setToMerge.Remove(toMerge);
                                // make sure to break out of all the loops to start over
                                changed = true;
                                break;
                            }
                        }
                        if (changed)
                        {
                            break;
                        }
                    }
                }
                // only stop looping when no more merges are performed
            } while (changed);
        }
    }
}
