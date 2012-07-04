﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace Model
{
    static class Extensions
    {
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static int GetAltHashCode<T>(this IList<T> listToHash) where T : IComparable
        {
            return listToHash.OrderBy(x => x).Aggregate(0, (x, y) => x.GetHashCode() ^ y.GetHashCode());
        }

        public static int GetAltHashCode<T>(this Stack<T> stackToHash) where T : IComparable
        {
            return stackToHash.OrderBy(x => x).Aggregate(0, (x, y) => x.GetHashCode() ^ y.GetHashCode());
        }

        public static Stack<T> Clone<T>(this Stack<T> stackToClone) where T : ICloneable
        {
            return new Stack<T>(stackToClone.Select(item => (T)item.Clone()).Reverse());
        }

        public static DateTime DateTimeCreator(int day, int hour, int minute)
        {
            DateTime now = DateTime.Now;
            DateTime to = new DateTime(now.Year, now.Month, (day % 365) + 1, hour, minute, 0);
            return to;
        }
    }
}