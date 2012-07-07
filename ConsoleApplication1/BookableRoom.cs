using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BookableRoom : ICloneable, IComparable
    {
        // Perhaps make a sorted list on start date ? 
        private Stack<TimeUnit> _freeTime;
        private Stack<TimeUnit> _takenTime;
        private Room _room;

        public Room Room
        {
            get { return (Room)_room.Clone(); }
            private set { _room = value == null ? new Room() : value; }
        }

        public Stack<TimeUnit> FreeTime
        {
            get { return _freeTime; }
            set { _freeTime = value == null ? new Stack<TimeUnit>() : value; }
        }

        public Stack<TimeUnit> TakenTime
        {
            get { return _takenTime; }
            set { _takenTime = value == null ? new Stack<TimeUnit>() : value; }
        }

        public BookableRoom(DateTime start, DateTime end, Room room)
            : this()
        {
            FreeTime.Push(new TimeUnit(start, end));
            Room = room;
        }

        public BookableRoom()
        {
            _freeTime = new Stack<TimeUnit>();
            _takenTime = new Stack<TimeUnit>();
            Room = new Room();
        }

        public bool CanFit(Course course)
        {
            if (!Room.CanFit(course))
            {
                return false;
            }
            if (!FreeTime.Any())
            {
                return false;
            }
            foreach (TimeUnit unit in FreeTime)
            {
                if (unit.Duration() < course.Duration)
                {
                    return false;
                }
            }
            return true;
        }

        public bool Fit(Course course)
        {

            bool result = false;
            if (CanFit(course))
            {
                TimeUnit before = FreeTime.Pop();
                SplitResult container = before.Split(course.Duration);
                TimeUnit taken = container.First;
                TimeUnit free = container.Second;
                taken.AssignedCourse = course;

                TakenTime.Push(taken);
                if (free != null)
                {
                    FreeTime.Push(free);
                }

                result = true;
            }
            return result;
        }

        public bool IsCourseBooked(Course course)
        {
            foreach (TimeUnit unit in TakenTime)
            {
                if (unit.AssignedCourse.Equals(course))
                {
                    return true;
                }
            }
            return false;
        }

        private bool AreStacksEqual(Stack<TimeUnit> first, Stack<TimeUnit> two)
        {
            Stack<TimeUnit> cloneThis = first.Clone();
            Stack<TimeUnit> cloneOther = two.Clone();
            while (cloneThis.Any())
            {
                if(!cloneThis.Pop().Equals(cloneOther.Pop())) {
                    return false;
                }
            }
            return true;
        }

        public bool Equals(BookableRoom other)
        {
            if (other == null)
            {
                return false;
            }
            if (!Room.Equals(other.Room))
            {
                return false;
            }
            if (other.FreeTime.Count != FreeTime.Count)
            {
                return false;
            }
            if (other.TakenTime.Count != TakenTime.Count)
            {
                return false;
            }
            if (!AreStacksEqual(FreeTime, other.FreeTime))
            {
                return false;
            }
            if (!AreStacksEqual(TakenTime, other.TakenTime))
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
            BookableRoom other = obj as BookableRoom;
            if (other == null)
            {
                return false;
            }
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Room.GetHashCode() ^ FreeTime.GetAltHashCode() ^ TakenTime.GetAltHashCode();
        }

        public object Clone()
        {
            BookableRoom clone = new BookableRoom();
            clone.FreeTime = FreeTime.Clone();
            clone.TakenTime = TakenTime.Clone();
            clone.Room = (Room)Room.Clone();
            return clone;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Format.TAB + Room);
            builder.AppendLine(Format.TAB + "Free time =");
            foreach (TimeUnit free in FreeTime.Reverse())
            {
                builder.AppendLine(Format.TAB + Format.TAB + free);
            }

            builder.AppendLine(Format.TAB + "Taken time=");
            foreach (TimeUnit taken in TakenTime.Reverse())
            {
                builder.AppendLine(Format.TAB + Format.TAB + taken);
            }

            return builder.ToString();
        }

        public int CompareTo(object obj)
        {
            BookableRoom other = obj as BookableRoom;
            return Room.CompareTo(other.Room);
        }
    }
}
