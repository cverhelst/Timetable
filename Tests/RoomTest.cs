using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Model;

namespace Tests
{
    [TestFixture]
    public class RoomTest
    {

        private Room room;
        private Resource resource1;
        private Resource resource2;
        private Resource resource3;
        private List<Resource> resources1;
        private List<Resource> resources2;
        private Course course1;
        private Course course2;

        [SetUp]
        public void init()
        {

            resource1 = new Resource("TV");
            resource2 = new Resource("Projector");
            resource3 = new Resource("PC");

            resources1 = new List<Resource>() { resource1, resource2 };
            resources2 = new List<Resource>() { resource1, resource3 };

            room = new Room(10, resources1);
            course1 = new Course(10, 10, resources1);
            course2 = new Course(10, 10, resources2);

        }

        [Test]
        public void Seats_GreaterThanZero_Pass()
        {
            room.Seats = 5;
            Assert.IsTrue(room.Seats == 5);
        }

        [Test]
        public void Seats_EqualToZero_Pass()
        {
            room.Seats = 0;
            Assert.IsTrue(room.Seats == 0);
        }

        [Test]
        public void Seats_LowerThanZero_SetZero()
        {
            room.Seats = -1;
            Assert.IsTrue(room.Seats == 0);
        }

        [Test]
        public void CanFit_TightestRequirements_Yes()
        {
            room.Seats = 10;
            room.Resources = resources1;

            course1.Students = 10;
            course1.RequiredResources = resources1;

            Assert.IsTrue(room.CanFit(course1));
        }

        [Test]
        public void CanFit_LooseRequirementsStudents_Yes()
        {
            room.Seats = 10;
            room.Resources = resources1;

            course1.Students = 1;
            course1.RequiredResources = resources1;

            Assert.IsTrue(room.CanFit(course1));
        }

        [Test]
        public void CanFit_LooseRequirementsResources_Yes()
        {
            room.Seats = 10;
            room.Resources = resources1;

            course1.Students = 10;
            course1.RequiredResources = new List<Resource>() { resources1.First() };

            Assert.IsTrue(room.CanFit(course1));
        }

        [Test]
        public void CanFit_TooManyStudents_No()
        {
            room.Seats = 10;
            room.Resources = resources1;

            course1.Students = 11;
            course1.RequiredResources = resources1;

            Assert.IsFalse(room.CanFit(course1));
        }

        [Test]
        public void CanFit_DifferentResources_No()
        {
            room.Seats = 10;
            room.Resources = resources1;

            course1.Students = 10;
            course1.RequiredResources = resources2;

            Assert.IsFalse(room.CanFit(course1));
        }

        [Test]
        public void Clone_DeepCopy()
        {
            room.Seats = 5;
            room.Resources = new List<Resource>() { resource1 };

            Room clone = (Room)room.Clone();

            // mutate seats
            clone.Seats = 10;
            // mutate resources list
            clone.Resources.Add(resource2);
            Resource resource = clone.Resources.First();
            // mutate resource in list
            resource.Name = "HighCeiling";

            Assert.AreEqual(5, room.Seats);
            Assert.AreEqual(1, room.Resources.Count);
            Assert.AreEqual("TV", room.Resources.First().Name);

            Assert.AreEqual(10, clone.Seats);
            Assert.AreEqual(2, clone.Resources.Count);
            Assert.AreEqual("HighCeiling",clone.Resources.First().Name);

        }
    }
}
