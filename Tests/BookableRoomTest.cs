﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Model;

namespace Tests
{
    [TestFixture]
    public class BookableRoomTest
    {
        private Room room;
        private BookableRoom broom1;
        private BookableRoom broom2;

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
            end = start.AddMinutes(60);

            broom1 = new BookableRoom(start, end, room);
            broom2 = new BookableRoom(start, end, room);

            course1 = new Course(1, 10, resources1);
            course2 = new Course(1, 20, resources2);

        }

        [Test]
        public void CanFit_RoomRequirementsFit_Yes()
        {
            // Course barely fits Room requirements
            Assert.IsTrue(broom1.CanFit(course1));
        }

        [Test]
        public void CanFit_RoomRequirementsDontFit_No()
        {
            // Course doesn't fit Room requirements
            Assert.IsFalse(broom1.CanFit(course2));
        }

        [Test]
        public void CanFit_SmallerDuration_Yes()
        {
            course1.Duration = 1;

            // Course fits Room and Duration requirements
            Assert.IsTrue(broom1.CanFit(course1));
        }

        [Test]
        public void CanFit_EqualDuration_Yes()
        {
            course1.Duration = 60;

            // Course barely fits Room and Duration requirements
            Assert.IsTrue(broom1.CanFit(course1));
        }

        [Test]
        public void CanFit_GreaterDuration_No()
        {
            course1.Duration = 120;

            // Course doesn't fit Time requirements
            Assert.IsFalse(broom1.CanFit(course1));
        }

        [Test]
        public void CanFit_NoFreeTimeLeft_No()
        {
            broom1.Time = new SortedSet<TimeUnit>();

            // There is no Free Time left to fit the course in
            Assert.IsFalse(broom1.CanFit(course1));
        }

        [Test]
        public void Fit_SmallerDuration_FreeTimeLeft()
        {
            course1.Duration = 1;

            // BRoom has fitted the course
            Assert.IsTrue(broom1.Fit(course1));

            // And will now have a timeunit equal to the leftover time
            var units = broom1.Time.Where(unit => unit.AssignedCourse == null).ToList();
            Assert.AreEqual(59, units.First().Duration());
            // As well as one for the booking
            units = broom1.Time.Where(unit => unit.AssignedCourse != null).ToList();
            Assert.AreEqual(1, units.First().Duration());
            Assert.AreEqual(course1, units.First().AssignedCourse);
        }

        [Test]
        public void Fit_EqualDuration_NoFreeTimeLeft()
        {
            course1.Duration = 60;

            // BRoom has fitted the course
            Assert.IsTrue(broom1.Fit(course1));

            // But there is no Free Time left
            var units = broom1.Time.Where(unit => unit.AssignedCourse == null).ToList();
            Assert.IsEmpty(units);
            // And the Taken Time reflects the fit
            units = broom1.Time.Where(unit => unit.AssignedCourse != null).ToList();
            Assert.AreEqual(60, units.First().Duration());
            // And has the course assigned
            Assert.AreEqual(course1, units.First().AssignedCourse);
        }

        [Test]
        public void Fit_GreaterDuration_No()
        {
            course1.Duration = 61;

            // The BRoom doesn't fit a course with too big duration
            Assert.IsFalse(broom1.Fit(course1));

            // Free Time stays equal
            Assert.AreEqual(60, broom1.Time.First().Duration());
            // and the course wasn't booked
            var units = broom1.Time.Where(unit => unit.AssignedCourse != null).ToList();
            Assert.IsEmpty(units);
        }

        [Test]
        public void IsCourseBooked_Yes()
        {
            Assert.IsTrue(broom1.Fit(course1));

            Assert.IsTrue(broom1.IsCourseBooked(course1));
            Assert.IsFalse(broom1.IsCourseBooked(course2));

            Course course3 = new Course(1, 1, null);
            Assert.IsTrue(broom1.Fit(course3));

            Assert.IsTrue(broom1.IsCourseBooked(course1));
            Assert.IsTrue(broom1.IsCourseBooked(course3));
            Assert.IsFalse(broom1.IsCourseBooked(course2));
        }

        [Test]
        public void Equals_Empty_Yes()
        {
            Assert.IsTrue(broom1.Equals(broom2));
        }

        [Test]
        public void Equals_BothBookedTheSame_IsEqual()
        {
            Assert.IsTrue(broom1.Fit(course1));
            Assert.IsTrue(broom2.Fit(course1));

            Assert.AreEqual(broom1, broom2);
        }

        [Test]
        public void Equals_EmptyAndDifferences_IsNotEqual()
        {
            broom2 = new BookableRoom(start, end, new Room(23, null));
            Assert.IsFalse(broom1.Equals(broom2));
        }

        [Test]
        public void Equals_OneHasBooked_IsNotEqual()
        {
            Assert.IsTrue(broom2.Fit(course1));
            Assert.IsFalse(broom1.Equals(broom2));
        }

        [Test]
        public void Equals_DifferentBookings_IsNotEqual()
        {
            room = new Room(20, resources1);
            course1 = new Course(1, 10, resources1);
            course2 = new Course(1, 20, null);
            broom1 = new BookableRoom(start, end, room);
            broom2 = new BookableRoom(start, end, room);
            Assert.IsTrue(broom1.Fit(course1));
            Assert.IsTrue(broom2.Fit(course2));

            Assert.AreNotEqual(broom1,broom2);
        }

        [Test]
        public void Equals_SameBookingsDifferentFreeTime_IsNotEqual()
        {
            room = new Room(20, resources1);
            course1 = new Course(1, 10, resources1);
            broom1 = new BookableRoom(start, end, room);
            broom2 = new BookableRoom(start, end.AddMinutes(5), room);

            Assert.IsTrue(broom1.Fit(course1));
            Assert.IsTrue(broom2.Fit(course1));

            Assert.AreNotEqual(broom1, broom2);

        }

        [Test]
        public void Equals_MultipleDifferentBookings_No()
        {
            room = new Room(20, resources1);
            course2 = new Course(1, 20, null);
            broom1 = new BookableRoom(start, end, room);
            broom2 = new BookableRoom(start, end, room);

            Assert.IsTrue(broom1.Fit(course1));
            Assert.IsTrue(broom1.Fit(course2));
            Assert.IsTrue(broom2.Fit(course1));

            Assert.AreNotEqual(broom1, broom2);
        }

        [Test]
        public void Clone_DeepCopy()
        {
            DateTime start2 = DateTime.Now.AddDays(1);
            DateTime end2 = start2.AddHours(2);
            Room room2 = new Room(40, resources2);

            BookableRoom clone = (BookableRoom)broom1.Clone();
            Assert.IsTrue(clone.Equals(broom1));

            BookableRoom helper = new BookableRoom(start2, end2, room2);
            // mutate 
            course1.Duration = 60;
            Assert.IsTrue(clone.Fit(course1));

            // Check original

            var units = broom1.Time.Where(unit => unit.AssignedCourse == null).ToList();
            Assert.AreEqual(60, units.First().Duration());
            units = broom1.Time.Where(unit => unit.AssignedCourse != null).ToList();
            Assert.IsEmpty(units);

            // Check clone
            units = clone.Time.Where(unit => unit.AssignedCourse == null).ToList();
            Assert.IsEmpty(units);
            units = clone.Time.Where(unit => unit.AssignedCourse != null).ToList();
            Assert.AreEqual(60, units.First().Duration());

        }

        [Test]
        public void GetHashCode_Clone_SameHashCode()
        {
            broom2 = (BookableRoom) broom1.Clone();

            Assert.AreEqual(broom1, broom2);
            Assert.AreEqual(broom1.GetHashCode(), broom2.GetHashCode());
        }

        [Test]
        public void GetHashCode_DifferentFields_DifferentHashCode()
        {
            broom2 = new BookableRoom(start.AddMinutes(1), end.AddMinutes(1), new Room("Test", 11, null));

            Assert.AreNotEqual(broom1, broom2);
            Assert.AreNotEqual(broom1.GetHashCode(), broom2.GetHashCode());
        }
    }
}
