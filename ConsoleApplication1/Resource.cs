using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Resource : ICloneable
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value == null ? "" : value; }
        }

        public Resource(string name)
        {
            Name = name;
        }

        public object Clone()
        {
            return new Resource(new String(name.ToCharArray()));
        }

        public bool Equals(Resource other)
        {
            if (other == null)
            {
                return false;
            }
            if (other.Name == Name)
            {
                return true;
            }
            return false;
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            Resource r = other as Resource;
            if (r == null)
            {
                return false;
            }
            return Equals(r);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
