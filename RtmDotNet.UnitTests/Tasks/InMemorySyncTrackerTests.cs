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
using RtmDotNet.Tasks;

namespace RtmDotNet.UnitTests.Tasks
{
    using NUnit.Framework;

    [TestFixture]
    public class InMemorySyncTrackerTests
    {
        [Test]
        public void GetSetLastSync_ValidDate_SupportsRoundtripRetrieval()
        {
            // Setup
            const string listId = "My Fake List Id";
            var expected = DateTime.Parse("2018-02-01 1:23:34pm");

            // Execute
            var syncTracker = GetItemUnderTest();
            syncTracker.SetLastSync(listId, expected);
            var actual = syncTracker.GetLastSync(listId);

            // Verify
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetLastSync_SetNotInvoked_ReturnsNull()
        {
            // Execute
            var syncTracker = GetItemUnderTest();
            var actual = syncTracker.GetLastSync("My Fake List ID");

            // Verify
            Assert.IsNull(actual);
        }

        [Test]
        public void GetLastSync_NullListId_ReturnsNull()
        {
            // Execute
            var syncTracker = GetItemUnderTest();
            var actual = syncTracker.GetLastSync(null);

            // Verify
            Assert.IsNull(actual);
        }

        [Test]
        public void SetLastSync_NullListId_ThrowsArgumentNullException()
        {
            // Execute
            var syncTracker = GetItemUnderTest();
            Assert.Throws<ArgumentNullException>(() => syncTracker.SetLastSync(null, DateTime.Now));
        }

        private InMemorySyncTracker GetItemUnderTest()
        {
            return new InMemorySyncTracker();
        }
    }
}