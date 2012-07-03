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

        public string Name { get; set; }

        public Room()
            : this(10, new List<Resource>())
        {
        }

        public Room(int seats, List<Resource> resources)
            : this("N/A", seats, resources)
        {
        }

        public Room(string name, int seats, List<Resource> resources)
        {
            Name = name;
            Seats = seats;
            Resources = resources;
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
            clone.Resources = (List<Resource>)Resources.Clone();
            clone.Name = Name;
            return clone;
        }
    }
}
