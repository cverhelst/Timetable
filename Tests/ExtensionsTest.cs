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
        }

        [Test]
        public void MergeUnits_3OrderedConsecutiveUnits_OneUnit()
        {
            units1 = units1.MergeUnits();

            Assert.AreEqual(1, units1.Count);
            Assert.AreEqual(start, units1.First().Start);
            Assert.AreEqual(end, units1.First().End);
        }
    }
}
