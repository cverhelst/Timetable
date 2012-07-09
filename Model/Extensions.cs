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

        public static int GetHashCodeOrderedCollection<T>(this ICollection<T> listToHash) where T : IComparable
        { 
            return listToHash.OrderBy(x => x).Aggregate(0, (x, y) => x.GetHashCode() ^ y.GetHashCode());
        }

        public static int GetHashCodeUnorderedCollection<T>(this ICollection<T> listToHash)
        {
            return listToHash.Aggregate(0, (x, y) => x.GetHashCode() * y.GetHashCode());
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

        public static bool UnorderedEquals<T>(this ICollection<T> a, ICollection<T> b)
        {
            // False if they have a different size
            if(a.Count != b.Count) {
                return false;
            }

            // Create a dictionary to store the item and their frequencies in the collections
            Dictionary<T, int> check = new Dictionary<T, int>();

            // Up the frequency for each item in a by one
            foreach (T item in a)
            {
                int frequency = 0;
                if (check.TryGetValue(item, out frequency))
                {
                    check[item] = frequency + 1;
                }
                else
                {
                    check.Add(item, 1);
                }
            }

            foreach (T item in b)
            {
                int frequency = 0;
                if(check.TryGetValue(item, out frequency)) {
                    // less frequent in a so lists aren't equal
                    if(frequency == 0) {
                        return false;
                    }
                    else {
                        check[item] = frequency - 1;
                    }
                } else {
                    // not present in a so lists aren't equal
                    return false;
                }
            }
            foreach (int frequency in check.Values) {
                // item was more frequent in b
                if(frequency != 0) {
                    return false;
                }
            }
            return true;
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
