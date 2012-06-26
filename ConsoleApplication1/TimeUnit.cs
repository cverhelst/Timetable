using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public struct TimeUnit
    {
        private DateTime _start;
        private DateTime _end;
        private Course _assignedCourse;

        public DateTime Start
        {
            get { return _start; }
            set
            {
                if (End != null && End.CompareTo(value) >= 0)
                {
                    _start = value;
                }
            }
        }

        public DateTime End
        {
            get { return _end; }
            set
            {
                if (Start != null && Start.CompareTo(value) <= 0)
                {
                    _end = value;
                }
            }
        }

        public Course AssignedCourse
        {
            get { return _assignedCourse; }
            set { _assignedCourse = value; }
        }

        public TimeUnit(DateTime start, DateTime end)
        {

            _start = DateTime.Now;
            _end = DateTime.Now;
            _assignedCourse = null;
            Start = start;
            End = end;
        }

        // Overlaps with other TimeUnit (end or start dates may be equal)
        public bool OverlapsWith(TimeUnit other)
        {
            bool result = false;
            result = true;
            // end2 <= start1
            if (Start.CompareTo(other.End) >= 0)
            {
                result = false;
            }
            // end1 <= start2
            else if (End.CompareTo(other.Start) <= 0)
            {
                result = false;
            }

            return result;
        }

        // Follows the other TimeUnit consecutively (the end of the other unit and the start of this unit are equal)
        public bool IsConsecutiveWith(TimeUnit other)
        {
            bool result = false;
            // end2 == start1
            if (Start.CompareTo(other.End) == 0)
            {
                result = true;
            }

            return result;
        }

        public SplitResult Split(int duration)
        {
            TimeUnit first = new TimeUnit(Start, Start.AddMinutes(duration));
            TimeUnit second = new TimeUnit(first.End, End);
            return new SplitResult(first, second);
        }

        public int Duration()
        {
            int result = 0;
            if (Start != null && End != null)
            {
                result = End.Subtract(Start).Minutes;
            }
            return result;
        }
    }

    public struct SplitResult
    {
        public TimeUnit First { get; set; }
        public TimeUnit Second { get; set; }

        public SplitResult(TimeUnit first, TimeUnit second) : this()
        {
            First = first;
            Second = second;
        }
    }
}
