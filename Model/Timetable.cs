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

        public Timetable(List<Day> days)
        {
            Days = days;
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
            Timetable clone = new Timetable((List<Day>)Days.Clone());
            return clone;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Format.DIVIDER);
            builder.AppendLine("Timetable");
            foreach(Day day in Days)
            {
                String dayInfo = day.ToString().Replace("\n", "\n" + Format.TAB);
                builder.AppendLine(Format.TAB + dayInfo);
            }
            return builder.ToString();
        }
    }
}
