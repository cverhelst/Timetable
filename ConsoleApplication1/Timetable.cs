using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Timetable : ICloneable
    {
        private List<Day> _days;

        public List<Day> Days
        {
            get { return _days; }
            set { _days = value == null ? new List<Day>() : value; }
        }

        public bool CanFit(Course course)
        {
            bool result = false;

            foreach (Day day in Days)
            {
                if (day.CanFit(course))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public bool Fit(Course course)
        {
            bool result = false;
            foreach (Day day in Days)
            {
                if (day.Fit(course))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public object Clone()
        {
            Timetable clone = new Timetable();
            clone.Days = (List<Day>) Days.Clone();
            return clone;
        }
    }
}
