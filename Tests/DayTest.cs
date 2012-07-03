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
        private List<BookableRoom> broomList;
        private Day day;

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

            broomList = new List<BookableRoom>() { broom1, broom2 };
            day = new Day(broomList);
        }

        [Test]
        public void CanFit_EmptyDay_Yes()
        {
            // Fits in broom 1
            Assert.IsTrue(day.CanFit(course1));

            // Fits in broom 2
            Assert.IsTrue(day.CanFit(course2));
        }

        [Test]
        public void CanFit_EmptyDay_No()
        {
            course1.Duration = 120;
            Assert.IsFalse(day.CanFit(course1));
        }

        [Test]
        public void Fit_EmptyDay_Yes()
        {
            // Fits in broom 1
            Assert.IsTrue(day.Fit(course1));
            Assert.IsTrue(day.IsCourseBooked(course1));
        }

        [Test]
        public void Fit_EmptyDay_No()
        {
            course1.Duration = 120;
            Assert.IsFalse(day.Fit(course1));
            Assert.IsFalse(day.IsCourseBooked(course1));
        }

        [Test]
        public void Fit_BusyDay_Yes()
        {
            
            course1.Duration = 1;
            Course course3 = (Course) course1.Clone();
            Assert.IsTrue(day.Fit(course1));
            Assert.IsTrue(day.Fit(course3));

            Assert.IsTrue(day.Rooms.First().IsCourseBooked(course1));
            Assert.IsTrue(day.Rooms.First().IsCourseBooked(course3));

            Assert.IsTrue(day.Fit(course2));
            
            Assert.IsTrue(day.Rooms[1].IsCourseBooked(course2));
        }

        [Test]
        public void Fit_ToFull_Yes()
        {
            end = start.AddMinutes(5);
            broom1 = new BookableRoom(start,end,room);
            broom2 = new BookableRoom(end.AddMinutes(60), end.AddMinutes(65), new Room(10,resources2));
            course1.Duration = 1;
            course2.Duration = 1;
            course1.Students = 1;
            course2.Students = 1;

            day = new Day(new List<BookableRoom>() { broom1, broom2});

            for(int i = 0; i<5; i++) {
                Assert.IsTrue(day.Fit(course1));
            }
            Assert.IsFalse(day.Fit(course1));
            for(int i = 0;i<5;i++) {
                Assert.IsTrue(day.Fit(course2));
            }
            Assert.IsFalse(day.Fit(course2));
        }

        [Test]
        public void Clone_DeepCopy()
        {
            Day clone = (Day) day.Clone();

            BookableRoom broom3 = new BookableRoom(start.AddMinutes(5), start.AddMinutes(10), new Room());
            
            // Mutate
            Assert.IsTrue(clone.Fit(course1));
            Assert.IsTrue(clone.IsCourseBooked(course1));
            Assert.IsFalse(day.IsCourseBooked(course1));
            clone.Number = 1;
            Assert.AreEqual(0, day.Number);
            Assert.AreEqual(1, clone.Number);

            clone.Rooms.Add(broom3);
            Assert.IsTrue(clone.Rooms.Contains(broom3));
            Assert.IsFalse(day.Rooms.Contains(broom3));
        }
    }
}
