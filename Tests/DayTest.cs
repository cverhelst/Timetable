using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class DayTest
    {
        private Room room;
        private BookableRoom broom1;
        private BookableRoom broom2;
        private List<BookableRoom> broomList1;
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
        private DateTime start;
        private DateTime end;

        [SetUp]
        public void init()
        {
            resource1 = new Resource("TV");
            resource2 = new Resource("Projector");
            resource3 = new Resource("PC");

            resources1 = new List<Resource>() { resource1, resource2 };
            resources2 = new List<Resource>() { resource1, resource3 };

            room = new Room(10, resources1);

            start = DateTime.Now;
            end = start.AddMinutes(30);

            broom1 = new BookableRoom(start, end, room);
            broom2 = new BookableRoom(start.AddMinutes(1), start.AddMinutes(41), new Room(20, resources2));

            course1 = new Course(30, 10, resources1);
            course2 = new Course(40, 20, resources2);

            broomList1 = new List<BookableRoom>() { broom1, broom2 };
            broomList2 = new List<BookableRoom>() { (BookableRoom)broom1.Clone(), (BookableRoom)broom2.Clone() };
            day1 = new Day(broomList1);
            day2 = new Day(broomList2);
        }

        [Test]
        public void CanFit_EmptyDay_Yes()
        {
            // Fits in broom 1
            Assert.IsTrue(day1.CanFit(course1));

            // Fits in broom 2
            Assert.IsTrue(day1.CanFit(course2));
        }

        [Test]
        public void CanFit_EmptyDay_No()
        {
            course1.Duration = 120;
            Assert.IsFalse(day1.CanFit(course1));
        }

        [Test]
        public void Fit_EmptyDay_Yes()
        {
            // Fits in broom 1
            Assert.IsTrue(day1.Fit(course1));
            Assert.IsTrue(day1.IsCourseBooked(course1));
        }

        [Test]
        public void Fit_EmptyDay_No()
        {
            course1.Duration = 120;
            Assert.IsFalse(day1.Fit(course1));
            Assert.IsFalse(day1.IsCourseBooked(course1));
        }

        [Test]
        public void Fit_BusyDay_Yes()
        {

            course1.Duration = 1;
            Course course3 = (Course)course1.Clone();
            Assert.IsTrue(day1.Fit(course1));
            Assert.IsTrue(day1.Fit(course3));

            Assert.IsTrue(day1.Rooms.First().IsCourseBooked(course1));
            Assert.IsTrue(day1.Rooms.First().IsCourseBooked(course3));

            Assert.IsTrue(day1.Fit(course2));

            Assert.IsTrue(day1.Rooms[1].IsCourseBooked(course2));
        }

        [Test]
        public void Fit_ToFull_Yes()
        {
            end = start.AddMinutes(5);
            broom1 = new BookableRoom(start, end, room);
            broom2 = new BookableRoom(end.AddMinutes(60), end.AddMinutes(65), new Room(10, resources2));
            course1.Duration = 1;
            course2.Duration = 1;
            course1.Students = 1;
            course2.Students = 1;

            day1 = new Day(new List<BookableRoom>() { broom1, broom2 });

            for (int i = 0; i < 5; i++)
            {
                Assert.IsTrue(day1.Fit(course1));
            }
            Assert.IsFalse(day1.Fit(course1));
            for (int i = 0; i < 5; i++)
            {
                Assert.IsTrue(day1.Fit(course2));
            }
            Assert.IsFalse(day1.Fit(course2));
        }

        [Test]
        public void Fit_OnlyOnce()
        {
            broom1 = new BookableRoom(start, end, room);
            broom2 = (BookableRoom)broom1.Clone();

            broomList1 = new List<BookableRoom>() { broom1, broom2 };
            day1 = new Day(broomList1);

            Assert.IsTrue(day1.Fit(course1));

            var fits = day1.Rooms.Where(roomX => roomX.IsCourseBooked(course1)).ToList();
            Assert.AreEqual(1, fits.Count);
        }

        [Test]
        public void Clone_DeepCopy()
        {
            Day clone = (Day)day1.Clone();

            BookableRoom broom3 = new BookableRoom(start.AddMinutes(5), start.AddMinutes(10), new Room());

            // Mutate
            Assert.IsTrue(clone.Fit(course1));
            Assert.IsTrue(clone.IsCourseBooked(course1));
            Assert.IsFalse(day1.IsCourseBooked(course1));
            clone.Number = 1;
            Assert.AreEqual(0, day1.Number);
            Assert.AreEqual(1, clone.Number);

            clone.Rooms.Add(broom3);
            Assert.IsTrue(clone.Rooms.Contains(broom3));
            Assert.IsFalse(day1.Rooms.Contains(broom3));
        }

        [Test]
        public void Equals_EmptyIdenticalRooms_Yes()
        {
            Assert.AreEqual(day1, day2);
        }

        [Test]
        public void Equals_FilledIdenticalRooms_Yes()
        {
            Assert.IsTrue(day1.Fit(course1));
            Assert.IsTrue(day2.Fit(course1));

            Assert.IsTrue(day1.Fit(course2));
            Assert.IsTrue(day2.Fit(course2));

            Assert.AreEqual(day1, day2);
        }

        [Test]
        public void Equals_DifferentRooms_No()
        {
            day2 = new Day(new List<BookableRoom>() { new BookableRoom(start, end, new Room("test", 10, null)) });
            Assert.AreNotEqual(day1, day2);
        }

        [Test]
        public void Equals_OnlyOneHasBookings_No()
        {
            Assert.IsTrue(day1.Fit(course1));

            Assert.AreNotEqual(day1, day2);
        }

        [Test]
        public void Equals_DifferentBookings_No()
        {
            Assert.IsTrue(day1.Fit(course1));
            Assert.IsTrue(day2.Fit(course2));

            Assert.AreNotEqual(day1, day2);
        }

        [Test]
        public void GetHashCode_Clone_SameHashCode()
        {
            day2 = (Day)day1.Clone();

            Assert.AreEqual(day1, day2);
            Assert.AreEqual(day1.GetHashCode(), day2.GetHashCode());
        }

        [Test]
        public void GetHashCode_DifferentFields_DifferentHashCode()
        {
            day1 = new Day(new List<BookableRoom>() { broom1 });
            day2 = new Day(new List<BookableRoom>() { broom2 });
            Assert.AreNotEqual(day1, day2);
            Assert.AreNotEqual(day1.GetHashCode(), day2.GetHashCode());
        }
    }
}
