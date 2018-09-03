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
using NUnit.Framework;
using RtmDotNet.Tasks;

namespace RtmDotNet.UnitTests.Tasks
{
    [TestFixture]
    public class RtmTaskTests
    {
        [TestCase("abc", "abc", true)]
        [TestCase("", "", true)]
        [TestCase(null, null, true)]
        [TestCase("abc", "ABC", false)]
        [TestCase("", "ABC", false)]
        [TestCase("abc", "", false)]
        [TestCase(null, "ABC", false)]
        [TestCase("abc", null, false)]
        public void Equals_RtmTask_ComparesTaskId(string id1, string id2, bool expected)
        {
            var task = new RtmTask(id1);
            var otherTask = new RtmTask(id2);

            var actual = task.Equals(otherTask);

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
        public void Equals_RtmTaskObject_ComparesTaskId(string id1, string id2, bool expected)
        {
            object task = new RtmTask(id1);
            object otherTask = new RtmTask(id2);

            var actual = task.Equals(otherTask);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Equals_NullTask_ReturnsFalse()
        {
            var task = GetItemUnderTest("abc");

            var actual = task.Equals(null);

            Assert.IsFalse(actual);
        }

        [Test]
        public void Equals_NonTask_ReturnsFalse()
        {
            var task = GetItemUnderTest("abc");

            // ReSharper disable once SuspiciousTypeConversion.Global
            var actual = task.Equals("abc");

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
        public void CompareTo_RtmTask_ComparesTaskId(string id1, string id2)
        {
            var task = new RtmTask(id1);
            var otherTask = new RtmTask(id2);
            var expected = string.Compare(id1, id2, StringComparison.Ordinal);

            var actual = task.CompareTo(otherTask);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CompareTo_Null_Returns1()
        {
            var task = new RtmTask("abc");

            var actual = task.CompareTo(null);

            Assert.AreEqual(1, actual);
        }

        [TestCase("abc")]
        [TestCase("123")]
        [TestCase("")]
        public void GetHashCode_RtmTask_ReturnsHashOfId(string id)
        {
            var task = new RtmTask(id);
            var expected = id.GetHashCode();

            var actual = task.GetHashCode();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetHashCode_NullId_Returns0()
        {
            var task = new RtmTask(null);

            var actual = task.GetHashCode();

            Assert.AreEqual(0, actual);
        }

        private RtmTask GetItemUnderTest(string id)
        {
            return new RtmTask(id);
        }
    }
}