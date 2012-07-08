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
        private Day day;
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

        private Timetable table;
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
            day = new Day(broomList);
            day2 = new Day(broomList2);

            table = new Timetable(new List<Day>() { day, day2 });
            table = new Timetable(new List<Day>() { day });
        }

        [Test]
        public void CanFit_CourseWillFitFirstDay_Yes()
        {
            Assert.IsTrue(table.CanFit(course1));
        }

        [Test]
        public void CanFit_CourseWillFitOnlyInLastDay_Yes()
        {
            course2.Students = 20;
            room2.Seats = 20;
            broom3 = new BookableRoom(end.AddMinutes(2), end.AddMinutes(7), room2);
            broomList2 = new List<BookableRoom>() { broom3 };
            day2 = new Day(broomList2);
            table = new Timetable(new List<Day>() { day, day2 });
            Assert.IsTrue(table.CanFit(course2));
        }

        [Test]
        public void CanFit_No()
        {
            Assert.IsFalse(table.CanFit(course3));
        }

        [Test]
        public void Fit_EmptyDays_Yes()
        {
            Assert.IsTrue(table.Fit(course1));
            Assert.IsTrue(table.Days.First().IsCourseBooked(course1));
        }

        [Test]
        public void Fit_BusyDay_Yes()
        {
            // All Will Fit the first day first room
            for (int i = 0; i < 5; i++)
            {
                Assert.IsTrue(table.Fit(course1));
            }
            var dayBooked = table.Days.Where(day => day.IsCourseBooked(course1)).ToList().First();
            var roomBooked = dayBooked.Rooms.Where(room => room.IsCourseBooked(course1)).ToList().First();
            Assert.AreEqual(5, roomBooked.Time.Count);

            Course course4 = (Course) course2.Clone();
            course4.Duration = 5;
            Assert.IsTrue(table.Fit(course4));

            dayBooked = table.Days.Where(day => day.IsCourseBooked(course4)).ToList().First();
            roomBooked = dayBooked.Rooms.Where(room => room.IsCourseBooked(course4)).ToList().First();
            Assert.AreEqual(1, roomBooked.Time.Count);

            for (int i = 0; i < 5; i++)
            {
                Assert.IsTrue(table.Fit(course2));
            }

            dayBooked = table.Days.Where(day => day.IsCourseBooked(course2)).ToList().First();
            roomBooked = dayBooked.Rooms.Where(room => room.IsCourseBooked(course2)).ToList().First();
            Assert.AreEqual(5, roomBooked.Time.Count);

            Assert.IsFalse(table.CanFit(course1));
            Assert.IsFalse(table.CanFit(course4));
            Assert.IsFalse(table.CanFit(course2));
        }

        [Test]
        public void Clone_DeepCopy()
        {
            Timetable clone = (Timetable) table.Clone();
            
            // Mutate
            Assert.IsTrue(clone.Days.First().Fit(course1));
            clone.Days.Add(new Day(new List<BookableRoom>()));

            Assert.IsFalse(table.Days.First().IsCourseBooked(course1));
            Assert.AreEqual(2, table.Days.Count);

            Assert.IsTrue(clone.Days.First().IsCourseBooked(course1));
            Assert.AreEqual(3, clone.Days.Count);
        }
    }
}
