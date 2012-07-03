using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    class CourseTest
    {
        private Resource resource1;
        private Resource resource2;
        private List<Resource> resources1;
        private List<Resource> resources2;
        private Course course1;

        [SetUp]
        public void init()
        {
            resource1 = new Resource("TV");
            resource2 = new Resource("Projector");
            resources1 = new List<Resource>() { resource1, resource2 };
            resources2 = new List<Resource>() { new Resource("TV"), new Resource("Projector") };
            course1 = new Course(10, 10, resources1);
        }

        [Test]
        public void Constructor_ResourcesNull_EmptyList()
        {
            course1 = new Course(10, 10, null);
            Assert.IsEmpty(course1.RequiredResources);
        }

        [Test]
        public void Equals_OtherObjectSameProperties_Yes()
        {
            Course course2 = new Course(10, 10, resources2);
            Assert.IsTrue(course1.Equals(course2));
        }

        [Test]
        public void Equals_Null_No()
        {
            Assert.IsFalse(course1.Equals(null));
        }

        [Test]
        public void Equals_OtherObjectDifferentProperties_No()
        {
            Course course2 = new Course(10, 10, new List<Resource>() { resource1 });
            Assert.IsFalse(course1.Equals(course2));

            course2.RequiredResources = new List<Resource>() { resource1, new Resource("Audio") };
            Assert.IsFalse(course1.Equals(course2));

            course2.RequiredResources = resources1;
            course2.Students = 11;
            Assert.IsFalse(course1.Equals(course2));

            course2.Students = 10;
            course2.Duration = 11;
            Assert.IsFalse(course1.Equals(course2));
        }

        [Test]
        public void Clone_DeepCopy()
        {
            Course clone = (Course) course1.Clone();
            Assert.AreEqual(course1, clone);

            // Mutate
            clone.Duration = 20;
            clone.Students = 20;
            clone.RequiredResources.First().Name = "Windows";
            clone.RequiredResources.Add(new Resource("Doors"));
            clone.Name = "Test";

            Assert.AreEqual(10, course1.Duration);
            Assert.AreEqual(10, course1.Students);
            Assert.AreEqual("TV", course1.RequiredResources.First().Name);
            Assert.AreEqual(2, course1.RequiredResources.Count);
            Assert.AreEqual("N/A", course1.Name);

            Assert.AreEqual(20, clone.Duration);
            Assert.AreEqual(20, clone.Students);
            Assert.AreEqual("Windows", clone.RequiredResources.First().Name);
            Assert.AreEqual(3, clone.RequiredResources.Count);
            Assert.AreEqual("Test", clone.Name);
        }
    }
}
