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
        // private List<TimeUnit> _assignedtimeUnits;

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

        //public List<TimeUnit> AssignedTimeUnits
        //{
        //    get { return _assignedtimeUnits; }
        //    set { _assignedtimeUnits = value == null ? new List<TimeUnit>() : value; }
        //}
        #endregion

        public Course()
            : this(120, 20, new List<Resource>())
        {
        }

        public Course(int duration, int students, List<Resource> requiredResources)
        {
            Duration = duration;
            Students = students;
            RequiredResources = requiredResources;
        }

        public bool Equals(object other)
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
            return new Course(Duration, Students, (List<Resource>) RequiredResources.Clone());
        }
    }
}
