using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class TimetableFitter
    {
        private Timetable _availabilityTimetable;
        private List<Timetable> _generatedTables;
        private List<Course> _courses;

        public Timetable AvailabilityTimetable
        {
            get { return _availabilityTimetable; }
            set { _availabilityTimetable = value; }
        }

        public List<Timetable> GeneratedTables
        {
            get { return _generatedTables; }
            set { _generatedTables = value == null ? new List<Timetable>() : value; }
        }

        public List<Course> Courses
        {
            get { return _courses; }
            set { _courses = value == null ? new List<Course>() : value; }
        }

        public List<Course> generateDefaultCourses()
        {
            // Resources
            Resource resource1 = new Resource("Projector");
            List<Resource> resources = new List<Resource>() { resource1 };

            // Courses
            Course course1 = new Course(240, 30, resources);
            Course course2 = new Course(120, 15, new List<Resource>());
            Course course3 = new Course(180, 15, new List<Resource>());
            List<Course> courses = new List<Course>() { course1, course2, course3 };
            return courses;

        }

        public Timetable generateDefaultTimeTable()
        {

            // Resources
            Resource resource1 = new Resource("Projector");
            List<Resource> resources = new List<Resource>() { resource1 };

            // Rooms
            Room room1 = new Room(30, resources);
            Room room2 = new Room(60, resources);

            // Set Room avalability
            BookableRoom book1 = new BookableRoom(Extensions.DateTimeCreator(8, 30), Extensions.DateTimeCreator(14, 0), room1);
            BookableRoom book2 = new BookableRoom(Extensions.DateTimeCreator(10, 30), Extensions.DateTimeCreator(16, 0), room2);
            List<BookableRoom> rooms1 = new List<BookableRoom>() { book1, book2 };
            List<BookableRoom> rooms2 = (List<BookableRoom>)rooms1.Clone();

            // Days
            Day day1 = new Day(rooms1);
            Day day2 = new Day(rooms2);

            // Timetable
            Timetable timetable = new Timetable();
            timetable.Days = new List<Day>() { day1, day2 };

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
        public void FitCourses(List<Course> courses, Timetable timetable)
        {
            if (courses.Any())
            {
                foreach (Course course in courses)
                {
                    // optimise for speed by combining check and actually fitting
                    if (timetable.CanFit(course))
                    {
                        // Not going to change the course itself, so copying the list is enough
                        List<Course> coursesLeft = new List<Course>(courses);
                        coursesLeft.Remove(course);
                        Timetable newTimetable = (Timetable)timetable.Clone();
                        newTimetable.Fit(course);
                        FitCourses(coursesLeft, newTimetable);
                    }
                }
            }
            else
            {
                GeneratedTables.Add(timetable);
            }
        }
    }
}
