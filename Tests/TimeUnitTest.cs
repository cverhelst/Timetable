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
        private TimeUnit unit1;
        private TimeUnit unit2;

        [SetUp]
        public void init()
        {
            start = DateTime.Now;
            end = start.AddHours(5);
            unit1 = new TimeUnit(start, end);
            unit2 = new TimeUnit(start, end);
        }

        [Test]
        public void Constructor_StartBeforeEnd_Pass()
        {
            unit1 = new TimeUnit(start, end);

            Assert.AreEqual(end, unit1.End);
            Assert.AreEqual(start, unit1.Start);
        }

        [Test]
        public void Constructor_StartEqualsEnd_Pass()
        {
            end = start;

            unit1 = new TimeUnit(start, end);

            Assert.AreEqual(end, unit1.End);
            Assert.AreEqual(start, unit1.Start);
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
        public void OverlapsWith_Partly_Yes()
        {
            TimeUnit unit2 = new TimeUnit(start.AddMinutes(1), end.AddMinutes(1));
            Assert.IsTrue(unit2.OverlapsWith(unit1));
            Assert.IsTrue(unit1.OverlapsWith(unit2));
        }

        [Test]
        public void OverlapsWith_Completely_Yes()
        {
            TimeUnit unit2 = new TimeUnit(start, end);
            Assert.IsTrue(unit1.OverlapsWith(unit2));
            Assert.IsTrue(unit2.OverlapsWith(unit1));
        }

        [Test]
        public void OverlapsWith_ConsecutiveUnits_No()
        {
            TimeUnit unit2 = new TimeUnit(end, end.AddDays(1));

            Assert.IsFalse(unit2.OverlapsWith(unit1));
            Assert.IsFalse(unit1.OverlapsWith(unit2));
        }

        [Test]
        public void OverlapsWith_GapBetweenUnits_No()
        {
            TimeUnit unit2 = new TimeUnit(end.AddHours(1), end.AddDays(1));

            Assert.IsFalse(unit2.OverlapsWith(unit1));
            Assert.IsFalse(unit1.OverlapsWith(unit2));
        }

        [Test]
        public void IsConsecutiveWith_Yes_ReturnTrue()
        {
            TimeUnit unit2 = new TimeUnit(end, end.AddDays(1));

            Assert.IsTrue(unit2.IsConsecutiveWith(unit1));
        }

        [Test]
        public void IsConsecutiveWith_ReversedOrder_ReturnFalse()
        {
            TimeUnit unit2 = new TimeUnit(end, end.AddDays(1));

            Assert.IsFalse(unit1.IsConsecutiveWith(unit2));
        }

        [Test]
        public void IsConsecutiveWith_GapBetweenTimeUnits_ReturnFalse()
        {
            TimeUnit unit2 = new TimeUnit(end.AddMilliseconds(1), end.AddDays(1));

            Assert.IsFalse(unit2.IsConsecutiveWith(unit1));
        }

        [Test]
        public void Split_SmallerPiece_ReturnPieceAndRemainder()
        {
            SplitResult split = unit1.Split(1);
            Split_Asserts(split, 1);
        }

        [Test]
        public void Split_EqualPiece_ReturnPieceAndNull()
        {
            SplitResult split = unit1.Split(unit1.Duration());

            Assert.AreEqual(unit1.Duration(), split.First.Duration());
            Assert.AreEqual(unit1.Start, split.First.Start);
            Assert.IsNull(split.Second);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Split_BiggerPiece_ArgumentException()
        {
            SplitResult split = unit1.Split(unit1.Duration() + 1);
        }

        private void Split_Asserts(SplitResult split, int dur)
        {
            Assert.AreEqual(dur, split.First.Duration());
            Assert.AreEqual(unit1.Start, split.First.Start);
            Assert.AreEqual(unit1.End, split.Second.End);

            Assert.IsTrue(split.Second.IsConsecutiveWith(split.First));
        }

        [Test]
        public void Clone_DeepCopy()
        {
            Resource resource = new Resource("TV");
            Course course = new Course(10, 10, new List<Resource>() { resource }) ;
            unit1.AssignedCourse = course;

            TimeUnit clone = (TimeUnit) unit1.Clone();

            Course course2 = new Course(20, 20, new List<Resource>());
            DateTime start2 = DateTime.Now.AddDays(1);
            DateTime end2 = start2.AddHours(1);
            
            // mutate course
            clone.AssignedCourse = course2;
            // mutate end
            clone.End = end2;
            // mutate start
            clone.Start = start2;

            Assert.AreEqual(course, unit1.AssignedCourse);
            Assert.AreEqual(start, unit1.Start);
            Assert.AreEqual(end, unit1.End);

            Assert.AreEqual(course2, clone.AssignedCourse);
            Assert.AreEqual(start2, clone.Start);
            Assert.AreEqual(end2, clone.End);
        }

        [Test]
        public void Equals_EverythingEqualNoCourse_Yes()
        {
            Assert.AreEqual(unit1, unit2);
        }

        [Test]
        public void Equals_EverythingEqualWithCourse_Yes()
        {
            unit1.AssignedCourse = new Course();
            unit2.AssignedCourse = new Course();

            Assert.AreEqual(unit1, unit2);
        }

        [Test]
        public void Equals_EverythingDifferentNoCourse_No()
        {
            unit2 = new TimeUnit(Extensions.DateTimeCreator(0, 8, 30), Extensions.DateTimeCreator(0, 9, 0));
            Assert.AreNotEqual(unit1, unit2);
        }

        [Test]
        public void Equals_EverythingEqualDifferentCourse_No()
        {
            unit1.AssignedCourse = new Course(10, 10, new List<Resource>() { new Resource("TV")});
            unit2.AssignedCourse = new Course(20, 20, null);

            Assert.AreNotEqual(unit1, unit2);
        }

        [Test]
        public void Equals_EverythingEqualOneCourseAssigned_No()
        {
            unit1.AssignedCourse = new Course();
            Assert.AreNotEqual(unit1, unit2);
            Assert.AreNotEqual(unit2, unit1);
        }

        [Test]
        public void Contains_Test()
        {
            List<TimeUnit> units = new List<TimeUnit>();
            units.Add(unit1);
            Assert.IsTrue(units.Contains(unit1));
            unit2 = new TimeUnit(unit1.Start, unit1.Start.AddHours(8));

            Assert.IsFalse(units.Contains(unit2));
        }
    }
}
