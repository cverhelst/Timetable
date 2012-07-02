using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Model;

namespace Tests
{
    [TestFixture]
    public class TimeUnitTest
    {
        private DateTime start;
        private DateTime end;
        private TimeUnit unit;

        [SetUp]
        public void init()
        {
            start = DateTime.Now;
            end = start.AddHours(5);
            unit = new TimeUnit(start, end);
        }

        [Test]
        public void Constructor_StartBeforeEnd_Pass()
        {
            
        }

        [Test]
        public void Constructor_StartEqualsEnd_Pass()
        {
            end = start;

            unit = new TimeUnit(start, end);

            Assert.AreEqual(end, unit.End);
            Assert.AreEqual(start, unit.Start);
        }

        // This test fail for example, replace result or delete this test to see all tests pass
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Constructor_StartNotBeforeEnd_ArgumentException()
        {
            DateTime end = DateTime.Now;
            DateTime start = end.AddDays(1);

            TimeUnit unit = new TimeUnit(start, end);
        }

        [Test]
        public void IsConsecutiveWith_Yes_ReturnTrue()
        {
            TimeUnit unit2 = new TimeUnit(end, end.AddDays(1));

            Assert.IsTrue(unit2.IsConsecutiveWith(unit));
        }

        [Test]
        public void IsConsecutiveWith_ReversedOrder_ReturnFalse()
        {
            TimeUnit unit2 = new TimeUnit(end, end.AddDays(1));

            Assert.IsFalse(unit.IsConsecutiveWith(unit2));
        }

        [Test]
        public void IsConsecutiveWith_GapBetweenTimeUnits_ReturnFalse()
        {
            TimeUnit unit2 = new TimeUnit(end.AddMilliseconds(1), end.AddDays(1));

            Assert.IsFalse(unit2.IsConsecutiveWith(unit));
        }

        [Test]
        public void Split_SmallerPiece_ReturnPieceAndRemainder()
        {
            SplitResult split = unit.Split(1);
            Split_Asserts(split, 1);
        }

        [Test]
        public void Split_EqualPiece_ReturnPieceAndNull()
        {
            SplitResult split = unit.Split(unit.Duration());

            Assert.AreEqual(unit.Duration(), split.First.Duration());
            Assert.AreEqual(unit.Start, split.First.Start);
            Assert.IsNull(split.Second);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Split_BiggerPiece_ArgumentException()
        {
            SplitResult split = unit.Split(unit.Duration() + 1);
        }

        private void Split_Asserts(SplitResult split, int dur)
        {
            Assert.AreEqual(dur, split.First.Duration());
            Assert.AreEqual(unit.Start, split.First.Start);
            Assert.AreEqual(unit.End, split.Second.End);

            Assert.IsTrue(split.Second.IsConsecutiveWith(split.First));
        }

        [Test]
        public void Clone_DeepCopy()
        {
            Resource resource = new Resource("TV");
            Course course = new Course(10, 10, new List<Resource>() { resource }) ;
            unit.AssignedCourse = course;

            TimeUnit clone = (TimeUnit) unit.Clone();

            Course course2 = new Course(20, 20, new List<Resource>());
            DateTime start2 = DateTime.Now.AddDays(1);
            DateTime end2 = start2.AddHours(1);
            
            // mutate course
            clone.AssignedCourse = course2;
            // mutate end
            clone.End = end2;
            // mutate start
            clone.Start = start2;

            Assert.AreEqual(course, unit.AssignedCourse);
            Assert.AreEqual(start, unit.Start);
            Assert.AreEqual(end, unit.End);

            Assert.AreEqual(course2, clone.AssignedCourse);
            Assert.AreEqual(start2, clone.Start);
            Assert.AreEqual(end2, clone.End);
        }
    }
}
