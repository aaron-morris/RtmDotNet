// -----------------------------------------------------------------------
//     This file is part of RtmDotNet.
//     Copyright (c) 2018 Aaron Morris
//     https://github.com/aaron-morris/RtmDotNet
// 
//     RtmDotNet is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     RtmDotNet is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
// 
//     You should have received a copy of the GNU General Public License
//     along with RtmDotNet.  If not, see <https://www.gnu.org/licenses/>.
// -----------------------------------------------------------------------

using System;
using RtmDotNet.Locations;

namespace RtmDotNet.UnitTests.Locations
{
    using NUnit.Framework;

    [TestFixture]
    public class RtmLocationTests
    {
        [TestCase("abc", "abc", true)]
        [TestCase("", "", true)]
        [TestCase(null, null, true)]
        [TestCase("abc", "ABC", false)]
        [TestCase("", "ABC", false)]
        [TestCase("abc", "", false)]
        [TestCase(null, "ABC", false)]
        [TestCase("abc", null, false)]
        public void Equals_RtmLocation_ComparesLocationId(string id1, string id2, bool expected)
        {
            var location = new RtmLocation(id1);
            var otherLocation = new RtmLocation(id2);

            var actual = location.Equals(otherLocation);

            Assert.AreEqual(expected, actual);
        }

        [TestCase("abc", "abc", true)]
        [TestCase("", "", true)]
        [TestCase(null, null, true)]
        [TestCase("abc", "xyz", false)]
        [TestCase("abc", "ABC", false)]
        [TestCase("", "ABC", false)]
        [TestCase("abc", "", false)]
        [TestCase(null, "ABC", false)]
        [TestCase("abc", null, false)]
        public void Equals_RtmLocationObject_ComparesLocationId(string id1, string id2, bool expected)
        {
            object location = new RtmLocation(id1);
            object otherLocation = new RtmLocation(id2);

            var actual = location.Equals(otherLocation);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Equals_NullLocation_ReturnsFalse()
        {
            var location = GetItemUnderTest("abc");

            var actual = location.Equals(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void Equals_NonLocation_ReturnsFalse()
        {
            var location = GetItemUnderTest("abc");

            // ReSharper disable once SuspiciousTypeConversion.Global
            var actual = location.Equals("abc");

            Assert.IsFalse(actual);
        }

        [TestCase("abc", "xyz")]
        [TestCase("xyz", "abc")]
        [TestCase("abc", "ABC")]
        [TestCase("ABC", "abc")]
        [TestCase("", "abc")]
        [TestCase("abc", "")]
        [TestCase(null, "abc")]
        [TestCase("abc", null)]
        [TestCase("", "")]
        [TestCase(null, null)]
        public void CompareTo_RtmLocation_ComparesLocationId(string id1, string id2)
        {
            var location = new RtmLocation(id1);
            var otherLocation = new RtmLocation(id2);
            var expected = string.Compare(id1, id2, StringComparison.Ordinal);

            var actual = location.CompareTo(otherLocation);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareTo_Null_Returns1()
        {
            var location = new RtmLocation("abc");

            var actual = location.CompareTo(null);

            Assert.AreEqual(1, actual);
        }

        [TestCase("abc")]
        [TestCase("123")]
        [TestCase("")]
        public void GetHashCode_RtmLocation_ReturnsHashOfId(string id)
        {
            var location = new RtmLocation(id);
            var expected = id.GetHashCode();

            var actual = location.GetHashCode();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetHashCode_NullId_Returns0()
        {
            var location = new RtmLocation(null);

            var actual = location.GetHashCode();

            Assert.AreEqual(0, actual);
        }

        private RtmLocation GetItemUnderTest(string id)
        {
            return new RtmLocation(id);
        }
    }
}