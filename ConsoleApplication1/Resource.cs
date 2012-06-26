using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public struct Resource
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value == null ? "" : value; }
        }

        public Resource(string name) : this()
        {
            Name = name;
        }
    }
}
