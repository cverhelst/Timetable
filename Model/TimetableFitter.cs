using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class TimetableFitter
    {
        private Timetable _availabilityTimetable;
        private HashSet<Timetable> _generatedTables;

        public Timetable AvailabilityTimetable
        {
            get { return _availabilityTimetable; }
            set { _availabilityTimetable = value; }
        }

        public HashSet<Timetable> GeneratedTables
        {
            get { return _generatedTables; }
            set { _generatedTables = value == null ? new HashSet<Timetable>() : value; }
        }

        public TimetableFitter()
        {
            GeneratedTables = new HashSet<Timetable>();
        }

        public List<Course> generateDefaultCourses()
        {
            // Resources
            Resource resource1 = new Resource("Projector");
            List<Resource> resources = new List<Resource>() { resource1 };

            // Courses
            Course course1 = new Course("Mathematics", 4 * 60, 5, resources);
            Course course2 = new Course("French", 2 * 60, 15, null);
            Course course3 = new Course("Programming", 4 * 60, 30, null);
            Course course4 = new Course("English", 1 * 60 + 30, 10, resources);
            Course course5 = new Course("English 2", 2 * 60, 5, resources);
            //List<Course> courses = new List<Course>() { course1, course2, course3, course4, course5 };
            List<Course> courses = new List<Course>() { course1 };
            return courses;
        }

        public Timetable generateDefaultTimeTable()
        {

            // Resources
            Resource resource1 = new Resource("Projector");
            List<Resource> resources = new List<Resource>() { resource1 };

            // Rooms
            Room room1 = new Room("001", 15, resources);
            Room room2 = new Room("112", 30, null);
            Room room3 = new Room("110", 35, resources);

            // Set Room avalability
            BookableRoom book1 = new BookableRoom(Extensions.DateTimeCreator(0, 8, 30), Extensions.DateTimeCreator(0, 14, 0), room1);
            BookableRoom book2 = new BookableRoom(Extensions.DateTimeCreator(0, 10, 30), Extensions.DateTimeCreator(0, 13, 0), room2);
            BookableRoom book3 = new BookableRoom(Extensions.DateTimeCreator(0, 8, 30), Extensions.DateTimeCreator(0, 14, 0), room3);
            List<BookableRoom> rooms1 = new List<BookableRoom>() { (BookableRoom)book1.Clone(), (BookableRoom)book2.Clone() };
            List<BookableRoom> rooms2 = new List<BookableRoom>() { (BookableRoom)book2.Clone(), (BookableRoom)book3.Clone() };
            List<BookableRoom> rooms3 = new List<BookableRoom>() { (BookableRoom)book1.Clone(), (BookableRoom)book3.Clone() };

            // Days
            Day day1 = new Day(1, rooms1);
            Day day2 = new Day(2, rooms2);
            Day day3 = new Day(3, rooms3);

            // Timetable
            Timetable timetable = new Timetable(new List<Day>() { day1, day2, day3 });

            return timetable;
        }

        /// <summary>
        /// list of courses
        /// timetable
        /// 
        /// for each course in the list of courses
        ///     try to fit this course in the timetable
        ///         fit is found: 
        ///             fit course in a copy of the timetable
        ///             make a copy of the list of courses and remove the reference to this course (shallow)
        ///             call this method with the copied list and timetable
        ///         fit is not found:
        ///             try the next course in the list of courses
        /// </summary>
        public bool FitCourses(List<Course> courses, Timetable timetable)
        {
            int before = GeneratedTables.Count;
            if (courses.Any())
            {
                foreach (Course course in courses)
                {
                    // TODO: optimise for speed by combining check and actually fitting
                    if (timetable.CanFit(course))
                    {
                        // Clone the list of courses left to fit
                        List<Course> coursesLeft = (List<Course>)courses.Clone();
                        // Remove the course that's going to be fitted from this cloned list
                        coursesLeft.Remove(course);
                        // Clone the timetable
                        Timetable newTimetable = (Timetable)timetable.Clone();
                        // Fit the course in the cloned timetable
                        newTimetable.Fit(course);
                        // Try to fit the remaining courses
                        FitCourses(coursesLeft, newTimetable);
                    }
                }
            }
            else
            {
                //bool present = false;
                //foreach (Timetable table in GeneratedTables)
                //{
                //    if (table.Equals(timetable))
                //    {
                //        present = true;
                //    }
                //}
                bool added = GeneratedTables.Add(timetable);
                //string output = String.Format("Item was already present? {0} and was added? {1}", present, added);
                //Console.Out.WriteLine(output);
            }
            return before < GeneratedTables.Count;
        }

        public void SqueezedFitCourses(List<Course> courses, Timetable timetable, int resolution)
        {
            bool allCoursesFitted = true;

            // We will keep squeezing the timetable as long as all the courses got fitted.
            while (allCoursesFitted)
            {
                // First we will get all the timeunits that exist in the timetable
                IEnumerable<TimeUnit> timeUnits =
                    (from day in timetable.Days
                         from room in day.Rooms
                             from time in room.Time
                             select time );
                // and find the one that has the largest duration.
                TimeUnit max = timeUnits.Aggregate((x, y) => x.Duration() > y.Duration() ? x : y);
                Logger.Log(String.Format("TimeUnit {0} ", max));
                // If this timeunit's duration is large enough the be squeezed, then squeeze.
                if (max.Shorten(resolution))
                {
                    Logger.Log(String.Format("has been shortened to {0}", max));
                    Timetable clone = (Timetable)timetable.Clone();
                    // We need to know if all the courses got fitted with this version of the timetable.
                    allCoursesFitted = FitCourses(courses, clone);
                }
                Logger.Log("could not be shortened further");
            }
        }

        private void PushedFitCourses(List<Course> courses, Timetable timetable, int resolution)
        {
            bool allCoursesFitted = false;

            // We will traverse the timetable one timeunit at a time.
            foreach (Day day in timetable.Days)
            {
                foreach (BookableRoom room in day.Rooms)
                {
                    foreach (TimeUnit time in room.Time)
                    {
                        // And should the timeunit's duration be large enough to be squeezed, we will squeeze it.
                        if (time.Shorten(resolution))
                        {
                            // And try to fit the courses again.
                            Timetable clone = (Timetable)timetable.Clone();
                            allCoursesFitted = FitCourses(courses, clone);
                            // In the event that not all courses got fitted, we will backtrack this last squeeze and move on
                            // to the next timeunit.
                            if (!allCoursesFitted)
                            {
                                time.Lengthen(resolution);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
