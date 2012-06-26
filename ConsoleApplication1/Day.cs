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

        public Day(List<BookableRoom> rooms)
        {

            _rooms = new List<BookableRoom>();
            Rooms = rooms;
            
        }

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

        public object Clone()
        {
            Day clone = new Day((List<BookableRoom>) Rooms.Clone());
            return clone;
        }
    }
}
