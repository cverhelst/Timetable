using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Course : ICloneable
    {
        #region Members
        private int _duration;
        private int _students;
        private List<Resource> _requiredResources;

        public int Duration
        {
            get { return _duration; }

            set { _duration = value < 0 ? 0 : value; }
        }

        public int Students
        {
            get { return _students; }
            set { _students = value < 0 ? 0 : value; }
        }

        public List<Resource> RequiredResources
        {
            get { return _requiredResources; }
            set { _requiredResources = value == null ? new List<Resource>() : value; }
        }

        public String Name { get; set; }
        #endregion

        public Course()
            : this(120, 20, new List<Resource>())
        {
        }

        public Course(int duration, int students, List<Resource> requiredResources)
            : this("N/A", duration, students, requiredResources)
        {
        }

        public Course(String name, int duration, int students, List<Resource> requiredResources)
        {
            Name = name;
            Duration = duration;
            Students = students;
            RequiredResources = requiredResources;
        }

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }
            Course c = other as Course;
            if (c == null)
            {
                return false;
            }
            return Equals(c);
        }

        public bool Equals(Course other)
        {
            if (other == null)
            {
                return false;
            }
            if (Students == other.Students
                && Duration == other.Duration
                && RequiredResources.SequenceEqual(other.RequiredResources))
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Students.GetHashCode() ^ Duration.GetHashCode() ^ RequiredResources.GetHashCode();
        }

        public object Clone()
        {
            return new Course(Name, Duration, Students, (List<Resource>)RequiredResources.Clone());
        }

        public override string ToString()
        {
            return DetailedInfo();
        }

        public string DetailedInfo()
        {
            StringBuilder builder = new StringBuilder();
            StringBuilder requirements = new StringBuilder();
            if (RequiredResources.Any())
            {
                foreach (Resource resource in RequiredResources)
                {
                    requirements.AppendFormat("{0} - ", resource);
                }
                requirements.Remove(requirements.Length - 3, 3);
            }
            builder.AppendFormat("Course {0}: Students({1}) | Duration({2} minutes) | Requirements({3})\n", Name, Students, Duration, requirements);
            return builder.ToString();
        }
    }
}
