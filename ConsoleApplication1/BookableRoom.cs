using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BookableRoom : ICloneable
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
            foreach(TimeUnit unit in TakenTime)
            {
                if(unit.AssignedCourse.Equals(course)) {
                    return true;
                }
            }
            return false;
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
            builder.AppendFormat("Room {0}\n", GetHashCode());
            builder.AppendFormat("\tSeats = {0}\n",Room.Seats);
            builder.AppendLine("\tFree time =");
            foreach(TimeUnit free in FreeTime) {
                builder.AppendFormat("\t\t{0} - {1}\n", free.Start, free.End);
            }
            builder.AppendLine("\tTaken time=");
            foreach (TimeUnit taken in TakenTime)
            {
                builder.AppendFormat("\t\t{0} - {1} booked course: {2}\n", taken.Start, taken.End, taken.AssignedCourse);
            }
            return builder.ToString();
        }
    }
}
