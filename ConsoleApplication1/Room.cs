using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Room : ICloneable
    {
        private int _seats;
        private List<Resource> _resources;

        public int Seats
        {
            get { return _seats; }
            set { _seats = value < 0 ? 0 : value; }
        }

        public List<Resource> Resources
        {
            get { return _resources; }
            set { _resources = value == null ? new List<Resource>() : value; }
        }

        public Room(int seats, List<Resource> resources)
        {
            Seats = seats;
            Resources = resources;
        }

        public Room()
            : this(10, new List<Resource>())
        {
        }

        public bool CanFit(Course course)
        {
            if (Seats < course.Students)
            {
                return false;
            }

            foreach (Resource resource in course.RequiredResources)
            {
                if (!Resources.Contains(resource))
                {
                    return false;
                }
            }
            return true;
        }

        public object Clone()
        {
            Room clone = new Room();
            clone.Seats = Seats;
            clone.Resources = Resources;
            return clone;
        }
    }
}
