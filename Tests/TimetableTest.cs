using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class TimetableTest
    {
        private Room room1;
        private Room room2;
        private BookableRoom broom1;
        private BookableRoom broom2;
        private BookableRoom broom3;
        private List<BookableRoom> broomList;
        private List<BookableRoom> broomList2;
        private Day day1;
        private Day day2;

        private Resource resource1;
        private Resource resource2;
        private Resource resource3;
        private List<Resource> resources1;
        private List<Resource> resources2;
        private Course course1;
        private Course course2;
        private Course course3;
        private DateTime start;
        private DateTime end;

        private Timetable table1;
        private Timetable table2;

        [SetUp]
        public void init()
        {
            resource1 = new Resource("TV");
            resource2 = new Resource("Projector");
            resource3 = new Resource("PC");

            resources1 = new List<Resource>() { resource1, resource2 };
            resources2 = new List<Resource>() { resource3 };

            room1 = new Room(10, resources1);
            room2 = new Room(10, resources2);

            start = DateTime.Now;
            end = start.AddMinutes(5);

            broom1 = new BookableRoom(start, end, room1);
            broom2 = new BookableRoom(end.AddMinutes(1), end.AddMinutes(6), room2);
            broom3 = new BookableRoom(end.AddMinutes(2), end.AddMinutes(7), room2);

            course1 = new Course(1, 10, resources1);
            course2 = new Course(1, 10, resources2);
            course3 = new Course(1, 20, new List<Resource>() { new Resource("CoffeeMachine")});

            broomList = new List<BookableRoom>() { broom1, broom2 };
            broomList2 = new List<BookableRoom>() { broom3 };
            day1 = new Day(broomList);
            day2 = new Day(broomList2);

            table1 = new Timetable(new List<Day>() { day1, day2 });
            table2 = new Timetable(new List<Day>() { day1 });
        }

        [Test]
        public void CanFit_CourseWillFitFirstDay_Yes()
        {
            Assert.IsTrue(table1.CanFit(course1));
        }

        [Test]
        public void CanFit_CourseWillFitOnlyInLastDay_Yes()
        {
            course2.Students = 20;
            room2.Seats = 20;
            broom3 = new BookableRoom(end.AddMinutes(2), end.AddMinutes(7), room2);
            broomList2 = new List<BookableRoom>() { broom3 };
            day2 = new Day(broomList2);
            table1 = new Timetable(new List<Day>() { day1, day2 });
            Assert.IsTrue(table1.CanFit(course2));
        }

        [Test]
        public void CanFit_No()
        {
            Assert.IsFalse(table1.CanFit(course3));
        }

        [Test]
        public void Fit_EmptyDays_Yes()
        {
            Assert.IsTrue(table1.Fit(course1));
            Assert.IsTrue(table1.Days.First().IsCourseBooked(course1));
        }

        [Test]
        public void Fit_BusyDay_Yes()
        {
            // All Will Fit the first day first room
            for (int i = 0; i < 5; i++)
            {
                Assert.IsTrue(table1.Fit(course1));
            }
            var dayBooked = table1.Days.Where(day => day.IsCourseBooked(course1)).ToList().First();
            var roomBooked = dayBooked.Rooms.Where(room => room.IsCourseBooked(course1)).ToList().First();
            Assert.AreEqual(5, roomBooked.Time.Count);

            Course course4 = (Course) course2.Clone();
            course4.Duration = 5;
            Assert.IsTrue(table1.Fit(course4));

            dayBooked = table1.Days.Where(day => day.IsCourseBooked(course4)).ToList().First();
            roomBooked = dayBooked.Rooms.Where(room => room.IsCourseBooked(course4)).ToList().First();
            Assert.AreEqual(1, roomBooked.Time.Count);

            for (int i = 0; i < 5; i++)
            {
                Assert.IsTrue(table1.Fit(course2));
            }

            dayBooked = table1.Days.Where(day => day.IsCourseBooked(course2)).ToList().First();
            roomBooked = dayBooked.Rooms.Where(room => room.IsCourseBooked(course2)).ToList().First();
            Assert.AreEqual(5, roomBooked.Time.Count);

            Assert.IsFalse(table1.CanFit(course1));
            Assert.IsFalse(table1.CanFit(course4));
            Assert.IsFalse(table1.CanFit(course2));
        }

        [Test]
        public void Clone_DeepCopy()
        {
            Timetable clone = (Timetable) table1.Clone();
            
            // Mutate
            Assert.IsTrue(clone.Days.First().Fit(course1));
            clone.Days.Add(new Day(new List<BookableRoom>()));

            Assert.IsFalse(table1.Days.First().IsCourseBooked(course1));
            Assert.AreEqual(2, table1.Days.Count);

            Assert.IsTrue(clone.Days.First().IsCourseBooked(course1));
            Assert.AreEqual(3, clone.Days.Count);
        }

        [Test]
        public void Equals_BothEmpty_Yes()
        {
            table1 = new Timetable(null);
            table2 = new Timetable(null);
            Assert.AreEqual(table1, table2);
        }

        [Test]
        public void Equals_SameDays_Yes()
        {
            day1 = new Day(broomList);
            day2 = new Day(broomList);

            table1 = new Timetable(new List<Day>() { day1 });
            table2 = new Timetable(new List<Day>() { day2 });

            Assert.AreEqual(table1, table2);
        }

        [Test]
        public void Equals_DifferentDays_No()
        {
            table1 = new Timetable(new List<Day>() { day2 });
            Assert.AreNotEqual(table1, table2);
        }

        [Test]
        public void Equals_DifferentAmountOfDays_No()
        {
            Assert.AreNotEqual(table1, table2);
        }

        [Test]
        public void Equals_Clone_Yes()
        {
            Assert.AreEqual(table1, (Timetable) table1.Clone());
        }

        [Test]
        public void Equals_InHashSet()
        {
            HashSet<Timetable> set = new HashSet<Timetable>();
            Assert.IsTrue(set.Add(table1));
            Assert.IsFalse(set.Add(table1));
            Assert.IsFalse(set.Add((Timetable)table1.Clone()));

            Assert.IsTrue(set.Add(table2));
            Timetable table3 = new Timetable(new List<Day>() { day1, day2 });
            Assert.IsFalse(set.Add(table3));

            Timetable table4 = new Timetable(new List<Day>() { (Day) day1.Clone(), (Day) day2.Clone() });
            Assert.IsFalse(set.Add(table4));

            Timetable table5 = (Timetable) table4.Clone();
            Timetable table6 = (Timetable) table4.Clone();
            table5.Fit(course1);
            table6.Fit(course1);
            Assert.IsTrue(set.Add(table5));
            Assert.IsFalse(set.Add(table6));
        }

        [Test]
        public void GetHashCode_Clone_SameHashCode()
        {
            table2 = (Timetable) table1.Clone();

            Assert.AreEqual(table1, table2);
            Assert.AreEqual(table1.GetHashCode(), table2.GetHashCode());
        }

        [Test]
        public void GetHashCode_DifferentFields_DifferentHashCode()
        {
            Assert.AreNotEqual(table1, table2);
            Assert.AreNotEqual(table1.GetHashCode(), table2.GetHashCode());
        }
    }
}
