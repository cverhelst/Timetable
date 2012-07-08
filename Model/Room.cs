using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Room : ICloneable, IComparable
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

        public Room() : this(10, new List<Resource>()) { }

        public Room(int seats, List<Resource> resources) : this("N/A", seats, resources) { }

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

        public bool Equals(Room other)
        {
            if (other == null)
            {
                return false;
            }
            if (Seats != other.Seats)
            {
                return false;
            }
            if (Resources.Count != other.Resources.Count)
            {
                return false;
            }
            if (Resources.Except(other.Resources).Any())
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
            Room other = obj as Room;
            if (other == null)
            {
                return false;
            }
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Seats.GetHashCode() ^ Resources.GetAltHashCode();
        }

        public object Clone()
        {
            Room clone = new Room();
            clone.Seats = Seats;
            clone.Resources = (List<Resource>)Resources.Clone();
            clone.Name = Name;
            return clone;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder res = new StringBuilder();
            if (Resources.Any())
            {
                foreach (Resource resource in Resources)
                {
                    res.AppendFormat("{0} - ", resource);
                }
                res.Remove(res.Length - 3, 3);
            }
            builder.AppendFormat("Room {0}: Seats({1}) | Resources({2})\n", Name, Seats, res);
            return builder.ToString();
        }

        public int CompareTo(Object obj)
        {
            Room room = obj as Room;
            return Seats.CompareTo(room.Seats);
        }
    }
}
