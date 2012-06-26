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

        public Room Room { get; set; }

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

        public BookableRoom(DateTime start, DateTime end, Room room) : this()
        {
            FreeTime.Push(new TimeUnit(start, end));
            Room = room;
        }

        public BookableRoom()
        {
            _freeTime = new Stack<TimeUnit>();
            _takenTime = new Stack<TimeUnit>();
        }

        public bool CanFit(Course course)
        {
            if (!Room.CanFit(course))
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
                FreeTime.Push(free);

                result = true;
            }
            return result;
        }

        public object Clone()
        {
            BookableRoom clone = new BookableRoom();
            clone.FreeTime = new Stack<TimeUnit>(FreeTime);
            clone.TakenTime = new Stack<TimeUnit>(TakenTime);
            clone.Room = Room;
            return clone;
        }
    }
}
