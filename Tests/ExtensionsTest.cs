using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class ExtensionsTest
    {
        private DateTime start;
        private DateTime point1;
        private DateTime point2;
        private DateTime end;

        private TimeUnit unit1;
        private TimeUnit unit2;
        private TimeUnit unit3;

        private SortedSet<TimeUnit> units1;
        private SortedSet<TimeUnit> units2;

        [SetUp]
        public void init()
        {
            start = Extensions.DateTimeCreator(0, 8, 30);
            point1 = Extensions.DateTimeCreator(0, 9, 0);
            point2 = Extensions.DateTimeCreator(0, 9, 30);
            end = Extensions.DateTimeCreator(0, 10, 0);

            unit1 = new TimeUnit(start, point1);
            unit2 = new TimeUnit(point1, point2);
            unit3 = new TimeUnit(point2, end);

            units1 = new SortedSet<TimeUnit>() { unit1, unit2, unit3 };
            units2 = new SortedSet<TimeUnit>();
        }

        [Test]
        public void MergeUnits_3OrderedConsecutiveUnits_OneUnit()
        {
            units1.MergeUnits();

            Assert.AreEqual(1, units1.Count);
            Assert.AreEqual(start, units1.First().Start);
            Assert.AreEqual(end, units1.First().End);
        }

        [Test]
        public void MergeUnits_EmptySet_Pass()
        {
            units2.MergeUnits();
        }

        [Test]
        public void MergeUnits_2ConsecutiveUnits1Not_OneUnit()
        {
            unit3 = new TimeUnit(end, end.AddMinutes(1));
            units1 = new SortedSet<TimeUnit>() { unit1, unit2, unit3 };

            units1.MergeUnits();

            Assert.AreEqual(2, units1.Count);
            Assert.AreEqual(start, units1.First().Start);
            Assert.AreEqual(point2, units1.First().End);
            Assert.AreEqual(end, units1.Last().Start);
        }

        [Test]
        public void MergeUnits_NoConsecutiveUnits_Pass()
        {
            unit1 = new TimeUnit(start, start.AddSeconds(1));
            unit2 = new TimeUnit(point1, point1.AddSeconds(1));
            unit3 = new TimeUnit(end, end.AddSeconds(1));

            units1 = new SortedSet<TimeUnit>() { unit1, unit2, unit3 };
            units1.MergeUnits();

            Assert.AreEqual(3, units1.Count);
        }

        [Test]
        public void MergeUnits_DontMergeBookedUnits_Pass()
        {
            unit3.AssignedCourse = new Course();
            units1 = new SortedSet<TimeUnit>() { unit1, unit2, unit3 };
            units1.MergeUnits();

            Assert.AreEqual(2, units1.Count);
            var units = units1.Where(unit => unit.AssignedCourse == null).ToList();
            Assert.AreEqual(1, units.Count);
            Assert.AreEqual(start, units.First().Start);
            Assert.AreEqual(point2, units.First().End);
        }
    }
}
