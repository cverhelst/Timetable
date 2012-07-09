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

        private Room room1;
        private Room room2;
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

            room1 = new Room(10, resources1);
            room2 = new Room(10, new List<Resource>() { new Resource("Projector"), new Resource("TV") });
            course1 = new Course(10, 10, resources1);
            course2 = new Course(10, 10, resources2);

        }

        [Test]
        public void Constructor_ResourcesNull_EmptyList()
        {
            room1 = new Room(10, null);
            Assert.IsEmpty(room1.Resources);
        }

        [Test]
        public void Seats_GreaterThanZero_Pass()
        {
            room1.Seats = 5;
            Assert.IsTrue(room1.Seats == 5);
        }

        [Test]
        public void Seats_EqualToZero_Pass()
        {
            room1.Seats = 0;
            Assert.IsTrue(room1.Seats == 0);
        }

        [Test]
        public void Seats_LowerThanZero_SetZero()
        {
            room1.Seats = -1;
            Assert.IsTrue(room1.Seats == 0);
        }

        [Test]
        public void CanFit_TightestRequirements_Yes()
        {
            room1.Seats = 10;
            room1.Resources = resources1;

            course1.Students = 10;
            course1.RequiredResources = resources1;

            Assert.IsTrue(room1.CanFit(course1));
        }

        [Test]
        public void CanFit_LooseRequirementsStudents_Yes()
        {
            room1.Seats = 10;
            room1.Resources = resources1;

            course1.Students = 1;
            course1.RequiredResources = resources1;

            Assert.IsTrue(room1.CanFit(course1));
        }

        [Test]
        public void CanFit_LooseRequirementsResources_Yes()
        {
            room1.Seats = 10;
            room1.Resources = resources1;

            course1.Students = 10;
            course1.RequiredResources = new List<Resource>() { resources1.First() };

            Assert.IsTrue(room1.CanFit(course1));
        }

        [Test]
        public void CanFit_TooManyStudents_No()
        {
            room1.Seats = 10;
            room1.Resources = resources1;

            course1.Students = 11;
            course1.RequiredResources = resources1;

            Assert.IsFalse(room1.CanFit(course1));
        }

        [Test]
        public void CanFit_DifferentResources_No()
        {
            room1.Seats = 10;
            room1.Resources = resources1;

            course1.Students = 10;
            course1.RequiredResources = resources2;

            Assert.IsFalse(room1.CanFit(course1));
        }

        [Test]
        public void Clone_DeepCopy()
        {
            room1.Seats = 5;
            room1.Resources = new List<Resource>() { resource1 };

            Room clone = (Room)room1.Clone();
            Assert.AreEqual(room1, clone);

            // mutate seats
            clone.Seats = 10;
            // mutate resources list
            clone.Resources.Add(resource2);
            Resource resource = clone.Resources.First();
            // mutate resource in list
            resource.Name = "HighCeiling";
            clone.Name = "001";

            Assert.AreEqual(5, room1.Seats);
            Assert.AreEqual(1, room1.Resources.Count);
            Assert.AreEqual("TV", room1.Resources.First().Name);
            Assert.AreEqual("N/A", room1.Name);

            Assert.AreEqual(10, clone.Seats);
            Assert.AreEqual(2, clone.Resources.Count);
            Assert.AreEqual("HighCeiling", clone.Resources.First().Name);
            Assert.AreEqual("001", clone.Name);

        }

        [Test]
        public void Equals_BasicStuffMatches_Yes()
        {
            room1.Resources = null;
            room2.Resources = null;
            Assert.IsTrue(room1.Equals(room2));
        }

        [Test]
        public void Equals_BasicStuffDiffers_No()
        {
            room1.Resources = null;
            room2.Resources = null;
            room1.Seats = 200;
            room1.Name = "thiswillnotmatch";
            Assert.IsFalse(room1.Equals(room2));
        }

        [Test]
        public void Equals_ResourcesMatch_Yes()
        {
            Assert.IsTrue(room1.Equals(room2));
        }

        [Test]
        public void Equals_ResourcesDiffer_No()
        {
            room1.Resources = resources2;
            Assert.IsFalse(room1.Equals(room2));
        }

        [Test]
        public void GetHashCode_Clone_SameHashCode() 
        {
            room2 = (Room) room1.Clone();
            Assert.AreEqual(room1, room2);
            Assert.AreEqual(room1.GetHashCode(), room2.GetHashCode());
        }

        [Test]
        public void GetHashCode_DifferentFields_DifferentHashCode()
        {
            room1 = new Room("Test", 2, new List<Resource>() { resource1 });
            room2 = new Room("Test2",3,null);

            Assert.AreNotEqual(room1, room2);
            Assert.AreNotEqual(room1.GetHashCode(), room2.GetHashCode());
        }
    }
}
