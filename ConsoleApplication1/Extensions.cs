using System;
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

        public static Stack<T> Clone<T>(this Stack<T> stackToClone) where T : ICloneable
        {
            return new Stack<T>(stackToClone.Select(item => (T)item.Clone()).Reverse());
        }

        public static DateTime DateTimeCreator(int hour, int minute)
        {
            DateTime now = DateTime.Now;
            DateTime to = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0);
            return to;
        }
    }
}
