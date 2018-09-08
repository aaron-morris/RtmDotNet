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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RtmDotNet.Locations;

namespace RtmDotNet.UnitTests.Locations
{
    using NUnit.Framework;

    [TestFixture]
    public class InMemoryLocationCacheTests
    {
        [Test]
        public async Task Roundtrip_EmptyCache_AddsTasksNew()
        {
            // Setup
            var fakeLocation1 = GetFakeLocation("Location 1");
            var fakeLocation2 = GetFakeLocation("Location 2");
            var fakeLocation3 = GetFakeLocation("Location 3");
            var locations = new List<IRtmLocation> { fakeLocation1, fakeLocation2, fakeLocation3 };

            // Execute
            var locationCache = GetItemUnderTest();
            await locationCache.AddOrReplaceAsync(locations).ConfigureAwait(false);
            var actual = await locationCache.GetAllAsync().ConfigureAwait(false);

            // Verify
            Assert.AreEqual(3, actual.Count);
            Assert.IsTrue(actual.Contains(fakeLocation1));
            Assert.IsTrue(actual.Contains(fakeLocation2));
            Assert.IsTrue(actual.Contains(fakeLocation3));
        }

        [Test]
        public async Task Roundtrip_ExistingEntries_UpdatesExisting()
        {
            // Setup
            var fakeLocation1 = GetFakeLocation("Location1", "Location 1");
            var fakeLocation2 = GetFakeLocation("Location2", "Location 2");
            var fakeLocation3 = GetFakeLocation("Location3", "Location 3");
            var locationBatch1 = new List<IRtmLocation> { fakeLocation1, fakeLocation2 };
            var locationBatch2 = new List<IRtmLocation> { fakeLocation2, fakeLocation3 };

            // Execute
            var locationCache = GetItemUnderTest();
            await locationCache.AddOrReplaceAsync(locationBatch1).ConfigureAwait(false);
            fakeLocation2.Name = "Location 2 - Updated";
            await locationCache.AddOrReplaceAsync(locationBatch2).ConfigureAwait(false);
            var actual = await locationCache.GetAllAsync().ConfigureAwait(false);

            // Verify
            Assert.AreEqual(3, actual.Count);

            var actual1 = actual.First(x => x.Id.Equals("Location1"));
            Assert.AreEqual("Location 1", actual1.Name);

            var actual2 = actual.First(x => x.Id.Equals("Location2"));
            Assert.AreEqual("Location 2 - Updated", actual2.Name);

            var actual3 = actual.First(x => x.Id.Equals("Location3"));
            Assert.AreEqual("Location 3", actual3.Name);
        }

        [Test]
        public async Task GetByIdAsync_IdExists_GetsLocation()
        {
            // Setup
            var fakeLocation1 = GetFakeLocation("Location 1");
            var fakeLocation2 = GetFakeLocation("Location 2");
            var fakeLocation3 = GetFakeLocation("Location 3");
            var locations = new List<IRtmLocation> { fakeLocation1, fakeLocation2, fakeLocation3 };

            // Execute
            var locationCache = GetItemUnderTest();
            await locationCache.AddOrReplaceAsync(locations).ConfigureAwait(false);
            var actual = await locationCache.GetByIdAsync("Location 2").ConfigureAwait(false);

            // Verify
            Assert.AreEqual(fakeLocation2, actual);
        }

        [Test]
        public async Task GetByIdAsync_IdDoesNotExists_GetsLocation()
        {
            // Setup
            var fakeLocation1 = GetFakeLocation("Location 1");
            var fakeLocation2 = GetFakeLocation("Location 2");
            var fakeLocation3 = GetFakeLocation("Location 3");
            var locations = new List<IRtmLocation> { fakeLocation1, fakeLocation2, fakeLocation3 };

            // Execute
            var locationCache = GetItemUnderTest();
            await locationCache.AddOrReplaceAsync(locations).ConfigureAwait(false);
            var actual = await locationCache.GetByIdAsync("Location x").ConfigureAwait(false);

            // Verify
            Assert.IsNull(actual);
        }

        [Test]
        public async Task ClearAsync_LocationsInCache_ClearsCache()
        {
            // Setup
            var fakeLocation1 = GetFakeLocation("Location 1");
            var fakeLocation2 = GetFakeLocation("Location 2");
            var fakeLocation3 = GetFakeLocation("Location 3");
            var locations = new List<IRtmLocation> { fakeLocation1, fakeLocation2, fakeLocation3 };

            // Execute
            var locationCache = GetItemUnderTest();
            await locationCache.AddOrReplaceAsync(locations).ConfigureAwait(false);
            await locationCache.ClearAsync().ConfigureAwait(false);
            var actual = await locationCache.GetAllAsync().ConfigureAwait(false);

            // Verify
            Assert.AreEqual(0, actual.Count);
        }

        private IRtmLocation GetFakeLocation(string locationId)
        {
            return new RtmLocation(locationId);
        }

        private IRtmLocation GetFakeLocation(string locationId, string locationName)
        {
            return new RtmLocation(locationId) {Name = locationName};
        }

        private InMemoryLocationCache GetItemUnderTest()
        {
            return new InMemoryLocationCache();
        }
    }
}