﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class BookableRoom : ICloneable, IComparable
    {
        // Perhaps make a sorted list on start date ? 
        private SortedSet<TimeUnit> _time;
        private Room _room;

        public Room Room
        {
            get { return (Room)_room.Clone(); }
            private set { _room = value == null ? new Room() : value; }
        }

        public SortedSet<TimeUnit> Time
        {
            get { return _time; }
            set { _time = value == null ? new SortedSet<TimeUnit>() : value; }
        }

        public BookableRoom(DateTime start, DateTime end, Room room) 
        {
            Time = new SortedSet<TimeUnit>();
            Time.Add(new TimeUnit(start, end));
            Room = room;
        }

        public BookableRoom()
        {
            Time = new SortedSet<TimeUnit>();
            Room = new Room();
        }

        private bool AreSetsEqual(ISet<TimeUnit> first, ISet<TimeUnit> two)
        {
            if(first.Count != two.Count) {
                return false;
            }
            foreach (TimeUnit unit in first)
            {
                if (!two.Contains(unit))
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanFit(Course course)
        {
            if (!Room.CanFit(course))
            {
                return false;
            }
            if (!Time.Any())
            {
                return false;
            }

            Time.MergeUnits();
            foreach (TimeUnit unit in Time)
            {
                if (unit.CanFit(course.Duration))
                {
                    return true;
                }
            }
            return false;
        }

        // TODO: Make inner split part an extension method for cleanliness
        public bool Fit(Course course)
        {
            bool result = false;
            if (CanFit(course))
            {
                foreach (TimeUnit unit in Time)
                {
                    if (unit.CanFit(course.Duration))
                    {
                        SplitResult split = unit.Split(course.Duration);
                        split.First.AssignedCourse = course;
                        Time.Remove(unit);
                        Time.Add(split.First);
                        if (split.Second != null)
                        {
                            Time.Add(split.Second);
                        }
                        result = true;
                        break;
                    }
                }
            }
            if (result)
            {
                Time.MergeUnits();
            }
            return result;
        }

        // TODO: Move to TimeUnit itself
        public bool IsCourseBooked(Course course)
        {
            foreach (TimeUnit unit in Time)
            {
                if (unit.AssignedCourse != null && unit.AssignedCourse.Equals(course))
                {
                    return true;
                }
            }
            return false;
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
            if (other.Time.Count != Time.Count)
            {
                return false;
            }
            if (!AreSetsEqual(Time,other.Time))
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
            return Room.GetHashCode() ^ Time.GetAltHashCode();
        }

        public object Clone()
        {
            BookableRoom clone = new BookableRoom();
            clone.Time = (SortedSet<TimeUnit>) Time.Clone();
            clone.Room = (Room)Room.Clone();
            return clone;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(Format.TAB + Room);
            builder.AppendLine(Format.TAB + "Bookings:");
            foreach (TimeUnit free in Time)
            {
                builder.AppendLine(Format.TAB + Format.TAB + free);
            }

            builder.AppendLine(Format.TAB + "Taken time=");
            foreach (TimeUnit taken in Time)
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
