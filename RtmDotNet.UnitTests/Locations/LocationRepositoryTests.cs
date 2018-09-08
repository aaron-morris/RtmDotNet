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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using RtmDotNet.Locations;

namespace RtmDotNet.UnitTests.Locations
{
    using NUnit.Framework;

    [TestFixture]
    public class LocationRepositoryTests
    {
        [Test]
        public async Task GetAllLocationsAsync_RefreshesCacheAndGetsAllLocations()
        {
            // Setup
            var fakeLocation1 = GetFakeLocation("Location 1");
            var fakeLocation2 = GetFakeLocation("Location 2");
            var fakeLocation3 = GetFakeLocation("Location 3");

            IList<IRtmLocation> fakeLocations = new List<IRtmLocation>{ fakeLocation1, fakeLocation2, fakeLocation3};
            var fakeApiClient = Substitute.For<ILocationApiClient>();
            fakeApiClient.GetAllLocationsAsync().Returns(Task.FromResult(fakeLocations));

            IList<IRtmLocation> expected = fakeLocations.ToList();
            var mockCache = Substitute.For<ILocationCache>();
            mockCache.GetAllAsync().Returns(Task.FromResult(expected));

            // Execute
            var repository = GetItemUnderTest(fakeApiClient, mockCache);
            var actual = await repository.GetAllLocationsAsync().ConfigureAwait(false);

            // Verify
            Assert.AreSame(expected, actual);

#pragma warning disable 4014
            mockCache.Received().ClearAsync();
            mockCache.Received().AddOrReplaceAsync(fakeLocations);
#pragma warning restore 4014
        }

        [Test]
        public async Task GetLocationByIdAsync_LocationIsInCache_DoesNotRefreshCache()
        {
            // Setup
            const string fakeLocationId = "Location 1";
            var expected = GetFakeLocation(fakeLocationId);

            var mockCache = Substitute.For<ILocationCache>();
            mockCache.GetByIdAsync(fakeLocationId).Returns(Task.FromResult(expected));

            // Execute
            var repository = GetItemUnderTest(null, mockCache);
            var actual = await repository.GetLocationByIdAsync(fakeLocationId).ConfigureAwait(false);

            // Verify
            Assert.AreSame(expected, actual);

#pragma warning disable 4014
            mockCache.DidNotReceive().ClearAsync();
            mockCache.DidNotReceive().AddOrReplaceAsync(Arg.Any<IEnumerable<IRtmLocation>>());
#pragma warning restore 4014
        }

        [Test]
        public async Task GetLocationByIdAsync_LocationIsNotInCache_RefreshesCache()
        {
            // Setup
            const string fakeLocationId = "Location 1";
            var expected = GetFakeLocation(fakeLocationId);

            IList<IRtmLocation> fakeLocations = new List<IRtmLocation> { expected };
            var fakeApiClient = Substitute.For<ILocationApiClient>();
            fakeApiClient.GetAllLocationsAsync().Returns(Task.FromResult(fakeLocations));

            var mockCache = Substitute.For<ILocationCache>();
            mockCache.GetByIdAsync(fakeLocationId).Returns(Task.FromResult((IRtmLocation)null), Task.FromResult(expected));

            // Execute
            var repository = GetItemUnderTest(fakeApiClient, mockCache);
            var actual = await repository.GetLocationByIdAsync(fakeLocationId).ConfigureAwait(false);

            // Verify
            Assert.AreSame(expected, actual);

#pragma warning disable 4014
            mockCache.Received().ClearAsync();
            mockCache.Received().AddOrReplaceAsync(fakeLocations);
#pragma warning restore 4014
        }

        [Test]
        public void GetLocationByIdAsync_LocationDoesNotExist_RefreshesCacheAndThrowsInvalidOperation()
        {
            // Setup
            const string fakeLocationId = "Location 1";
            var expected = GetFakeLocation(fakeLocationId);

            IList<IRtmLocation> fakeLocations = new List<IRtmLocation> { expected };
            var fakeApiClient = Substitute.For<ILocationApiClient>();
            fakeApiClient.GetAllLocationsAsync().Returns(Task.FromResult(fakeLocations));

            var mockCache = Substitute.For<ILocationCache>();
            mockCache.GetByIdAsync(fakeLocationId).Returns(Task.FromResult((IRtmLocation)null), Task.FromResult((IRtmLocation)null));

            // Execute
            var repository = GetItemUnderTest(fakeApiClient, mockCache);
            Assert.ThrowsAsync<InvalidOperationException>( () => repository.GetLocationByIdAsync(fakeLocationId));

            // Verify
#pragma warning disable 4014
            mockCache.Received().ClearAsync();
            mockCache.Received().AddOrReplaceAsync(fakeLocations);
#pragma warning restore 4014
        }

        [TestCase("")]
        [TestCase(null)]
        public void GetLocationByIdAsync_LocationIdIsNull_ThrowsArgumentException(string locationId)
        {
            var repository = GetItemUnderTest();
            Assert.ThrowsAsync<ArgumentNullException>(() => repository.GetLocationByIdAsync(locationId));
        }

        private IRtmLocation GetFakeLocation(string id)
        {
            return new RtmLocation(id);
        }

        private LocationRepository GetItemUnderTest()
        {
            return GetItemUnderTest(Substitute.For<ILocationApiClient>(), Substitute.For<ILocationCache>());
        }

        private LocationRepository GetItemUnderTest(ILocationApiClient apiClient, ILocationCache cache)
        {
            return new LocationRepository(apiClient, cache);
        }
    }
}