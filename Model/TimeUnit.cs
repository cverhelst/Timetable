using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class TimeUnit : ICloneable, IComparable
    {
        private DateTime _start;
        private DateTime _end;
        private Course _assignedCourse;

        public DateTime Start
        {
            get { return _start; }
            set
            {
                if (End.CompareTo(value) >= 0)
                {
                    _start = value;
                }
            }
        }

        public DateTime End
        {
            get { return _end; }
            set {
                if (Start.CompareTo(value) <= 0)
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
            init(start, end);
        }

        private void init(DateTime start, DateTime end)
        {
            if (end.CompareTo(start) < 0)
            {
                throw new ArgumentException("End must be higher or equal to Start");
            }
            else
            {
                _start = start;
                _end = end;
            }
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

        // Splits the TimeUnit to return two other TimeUnits of which the first is equal to the requested duration
        // and the second is the remainder
        public SplitResult Split(int duration)
        {
            if (duration > Duration())
            {
                throw new ArgumentException("parameter must be lower than or equal than the TimeUnit's duration");
            }
            else if (duration < Duration())
            {
                TimeUnit first = new TimeUnit(Start, Start.AddMinutes(duration));
                TimeUnit second = new TimeUnit(first.End, End);
                return new SplitResult(first, second);
            }
            else
            {
                return new SplitResult(this, null);
            }
        }

        public bool Merge(TimeUnit other)
        {
            bool result = false;
            if(other.IsConsecutiveWith(this)) {
                End = other.End;
                result = true;
            }
            return result;
        }

        public int Duration()
        {
            return (int) End.Subtract(Start).TotalMinutes;
        }

        public bool Equals(TimeUnit other)
        {
            if (other == null)
            {
                return false;
            }
            if (!Start.Equals(other.Start))
            {
                return false;
            }
            if (!End.Equals(other.End))
            {
                return false;
            }
            if (AssignedCourse != null)
            {
                if (!AssignedCourse.Equals(other.AssignedCourse))
                {
                    return false;
                }
            }
            else if (other.AssignedCourse != null)
            {
                return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            TimeUnit other = obj as TimeUnit;
            if (other == null)
            {
                return false;
            }
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }

        public object Clone()
        {
            TimeUnit unit = new TimeUnit(Start, End);
            unit.AssignedCourse = AssignedCourse == null ? null : (Course) AssignedCourse.Clone();
            return unit;
        }

        // Orders first according to Start time, afterwards End time, afterwards on Assigned Course's properties
        // The shortest TimeUnit will be ordered before another TimeUnit that has the same Start time
        public int CompareTo(object obj)
        {
            TimeUnit other = obj as TimeUnit;
            return Detail().CompareTo(other.Detail());
        }

        public override string ToString()
        {
            return String.Format("{0} - {1} booked course: {2}", Start, End, AssignedCourse);
        }

        public string Detail()
        {
            return String.Format("{0}{1}{2}",Start.ToString("HH:mm:ss"),End.ToString("HH:mm:ss"),AssignedCourse);
        }

        public bool CanFit(int dur)
        {
            if (AssignedCourse != null)
            {
                return false;
            }
            if (Duration() < dur)
            {
                return false;
            }
            return true;
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
