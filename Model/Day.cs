using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Day : ICloneable
    {
        private List<BookableRoom> _rooms;

        public List<BookableRoom> Rooms
        {
            get { return _rooms; }
            set { _rooms = value == null ? new List<BookableRoom>() : value; }
        }

        public int Number { get; set; }

        public Day(int number, List<BookableRoom> rooms)
        {
            Number = number;
            Rooms = rooms;
        }

        public Day(List<BookableRoom> rooms) : this(0, rooms) { }

        public bool CanFit(Course course)
        {
            bool result = false;
            foreach (BookableRoom room in Rooms)
            {
                if (room.CanFit(course))
                {
                    result = true;
                }
            }
            return result;
        }

        public bool Fit(Course course)
        {
            bool result = false;
            foreach (BookableRoom room in Rooms)
            {
                if (room.Fit(course))
                {
                    result = true;
                }
            }
            return result;
        }

        public bool IsCourseBooked(Course course)
        {
            bool result = false;
            foreach (BookableRoom room in Rooms)
            {
                if (room.IsCourseBooked(course))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public object Clone()
        {
            Day clone = new Day(Number, (List<BookableRoom>)Rooms.Clone());
            return clone;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("Day {0}\n", Number);
            foreach(BookableRoom room in Rooms)
            {
                String roomInfo = room.ToString().Replace("\n", "\n" + Format.TAB);
                builder.AppendLine(Format.TAB + roomInfo);
            }
            return builder.ToString();
        }

        public bool Equals(Day other)
        {
            if (other == null)
            {
                return false;
            }
            if (Number != other.Number)
            {
                return false;
            }
            if (Rooms.Count != other.Rooms.Count)
            {
                return false;
            }
            if (Rooms.Except(other.Rooms).Any())
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
            Day other = obj as Day;
            if (other == null)
            {
                return false;
            }
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Rooms.GetHashCodeOrderedCollection();
        }
    }
}
